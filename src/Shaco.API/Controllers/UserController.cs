using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shaco.API.Models;
using Shaco.API.Services;

namespace Shaco.API.Controllers;

[Route("User")]
public class UserController(
  UserService userService,
  TokenService tokenService
) : Controller { 

  private readonly UserService _us = userService;
  private readonly TokenService _ts = tokenService;

  [HttpPost]
  public IActionResult CreateUser(
    [FromBody]UserAuth data
  ) {
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

    var token = _ts.CreateToken(user);
    Response.Headers["X-Token"] = token;
    return base.Ok();
  }

  [HttpGet]
  [Authorize(Roles="Admin")]
  public IActionResult GetUsers() =>
    base.Ok(_us.GetAll());

  [HttpGet("{id}")]
  [Authorize(Roles="Admin")]
  public IActionResult GetUser(Guid id) {
    if(_us.tryGetByID(id) is not User user)
      return base.NotFound();
    
    return base.Ok(user);
  }
  
  [HttpDelete("{id}")]
  [Authorize(Roles="Admin")]
  public IActionResult DeleteUser(Guid id) {
    if(_us.tryGetByID(id) is User user)
      _us.DeleteUser(user);

    return base.Ok();
  }

}
