using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shaco.API.Data;
using Shaco.API.Models;

namespace Shaco.API.Controllers;

[Authorize]
[Route("Link")]
public class LinkController(
  DB db,
  ILogger<LinkController> logger
) : Controller {

  private readonly DB _db = db;
  private readonly ILogger _logger = logger;

  [HttpGet]
  public IActionResult Test() {
    var links = _db.Links.ToArray();
    return base.Ok(links);
  }

  [HttpPost]
  public IActionResult CreateNewLink(
    [FromBody]PostLink data
  ) {
    var id = Guid.NewGuid();
    
    _logger.LogInformation("Create new link {id} > {dest}", id, data.Url);
    var link = _db.Links.Add(new Link {
      ID = id,
      Name = string.IsNullOrWhiteSpace(data.Name) ? id.ToString() : data.Name,
      Url = data.Url.AbsoluteUri
    }).Entity;

    _db.SaveChanges();
    return base.Ok(link);
  }

  [HttpGet("{id}")]
  public IActionResult GetLink(
    Guid id
  ) {
    if(_db.Links.SingleOrDefault(e => e.ID == id) is not Link link)
      return base.NotFound();

    return base.Ok(link);
  }

  [HttpDelete("{id}")]
  public IActionResult DeleteLink(
    Guid id
  ) {
  
    if(_db.Links.SingleOrDefault(e => e.ID == id) is Link link) {
      _logger.LogInformation("Delete link {id}", link.ID);
      _db.Links.Remove(link);
      _db.SaveChanges();
    }

    _db.SaveChanges();
    return base.Ok();

  }

}
