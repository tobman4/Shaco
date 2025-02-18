using Microsoft.AspNetCore.Mvc;
using Shaco.API.Data;

namespace Shaco.API.Controllers;

[Route("r")]
public class RedirectController(
  DB db
) : Controller {

  private readonly DB _db = db;

  [HttpGet("{name}")]
  public IActionResult FollowLink(string name) {
    if(_db.Links.SingleOrDefault(e => e.Name == name) is not Link link)
      return base.NotFound();

    return base.RedirectPermanent(link.Url);
  }

}
