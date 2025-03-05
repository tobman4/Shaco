using Microsoft.AspNetCore.Mvc;
using Shaco.API.Data;

namespace Shaco.API.Controllers;

[Route("r")]
public class RedirectController(
  DB db,
  ILogger<RedirectController> logger
) : Controller {

  private readonly DB _db = db;
  private readonly ILogger _logger = logger;

 [HttpGet("{name}")]
 public IActionResult FollowLink(string name) {
   var isDisc = Request
     .Headers
     .UserAgent
     .Single()?
     .ToString()
     .ToUpper()
     .Contains("DISCORD");

    if(isDisc == true)
      return base.Ok();

    if(_db.Links.SingleOrDefault(e => e.Name == name) is not Link link)
      return base.NotFound();
    
    var ip = Request.Headers["X-Forwarded-For"].ToString();
    if(!string.IsNullOrWhiteSpace(ip))
      _logger.LogDebug("{ip} got {link}", ip, link.Name);

    return base.RedirectPermanent(link.Url);
  }

}
