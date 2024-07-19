
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shaco.Contract.DTOs;

namespace Shaco.Api.Controllers;

[Route("api/Link")]
[Controller]
public class LinkController : Controller {
  private readonly ILogger<LinkController> _logger;
  private readonly ILinkRepo _repo;

  public LinkController(
    ILogger<LinkController> logger,
    ILinkRepo repo
  ) {
    _logger = logger;
    _repo = repo;
  }
 

  [Authorize("Admin")]
  [HttpGet]
  public IActionResult GetAll() {
    return base.Ok(_repo.GetAll());
  }

  [HttpGet("f/{key}")]
  public IActionResult GoTo([FromRoute]string key) {
    var userAgent = Request.Headers.UserAgent.FirstOrDefault();
    
    if(userAgent is not null) {
      if(userAgent.Contains("Discordbot")) return base.Ok();
    }

    if(_repo.GetOne(key) is not LinkDTO link) return base.NotFound();
    return base.Redirect(link.Uri.AbsoluteUri);
  }
  
  [HttpGet("{key}")]
  public IActionResult Get([FromRoute]string key) {
    if(_repo.GetOne(key) is not LinkDTO link) return base.NotFound();

    return base.Ok(link);
  }

  [Authorize("Admin")]
  [HttpPost]
  public IActionResult Post([FromBody]NewLinkDTO link) {
    var id = Guid.NewGuid();
    string? name = link.Name;

    if(Guid.TryParse(name, out Guid _)) return base.BadRequest("Name canot be a valid guid");
    if(!link.Dest.IsAbsoluteUri) return base.BadRequest("Bad dest.");

    if(string.IsNullOrWhiteSpace(name)) name = id.ToString();

    if(_repo.ByName(name) is not null) return base.Conflict("Name is in use");

    var res = _repo.Add(new LinkDTO {
      ID = id,
      Name = name,
      Uri = link.Dest
    });

    _repo.Save();
    
    _logger.LogInformation("Create link: {name}({id})", res.Name, res.ID);
    return base.Ok(res);
  }

  [Authorize("Admin")]
  [HttpDelete("{key}")]
  public async Task<IActionResult> Delete(string key) {
    if(_repo.GetOne(key) is not LinkDTO link) return base.NotFound();
    _repo.Delete(link.ID);

    await _repo.SaveAsync();

    _logger.LogInformation("Delete key {key}",key);
    return base.Ok();
  }
}
