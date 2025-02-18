using Microsoft.AspNetCore.Mvc;
using Shaco.API.Models;
using Shaco.API.Services;

namespace Shaco.API.Controllers;

[Route("User")]
public class UserController(
  UserService userService
) : Controller { 

  private readonly UserService _us = userService;


  [HttpPost]
  public IActionResult CreateUser(
    [FromBody]UserAuth data
  ) {
    Console.WriteLine(data.Username);
    Console.WriteLine(data.Password);
    var user = _us.AddUser(data.Username, data.Password);
    return base.Ok();
  }

  [HttpPost("Login")]
  public IActionResult Login(
    [FromBody]UserAuth data
  ) {
    var user = _us.TryAuth(data.Username, data.Password);
    if(user is null)
      return base.Unauthorized();

    //TODO: Create token
    return base.Ok();
  }

}
