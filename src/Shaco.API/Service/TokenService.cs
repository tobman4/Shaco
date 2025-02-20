using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Shaco.API.Services;

public class TokenService {

  private readonly SigningCredentials _signCrets;

  public TokenService(
    IConfiguration config
  ) {
    
    var password = config["Secret"];
    if(string.IsNullOrWhiteSpace(password))
      throw new Exception("Missing secret");

    _signCrets = new SigningCredentials(
      new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(password)
      ),
      SecurityAlgorithms.HmacSha256
    );

  }

  public string CreateToken(User user) {
    var token = new JwtSecurityToken(
      issuer: "Shaco.API",
      audience: "Shaco.API",
      expires: DateTime.Now.AddHours(12),
      signingCredentials: _signCrets,
      claims: new List<Claim> {
        new("Role", user.Role),
        new("User", user.Username)
      }
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

}
