using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Shaco.Api.Authentication;

public class BasicAuthenticationOpt : AuthenticationSchemeOptions {
}

public class BasicAuthentication : AuthenticationHandler<BasicAuthenticationOpt> {
  private readonly ILogger<BasicAuthentication> _logger;

  private readonly string _password = "47C06890F1915956B82C1CCD5D90950ECB8F8E45573CAFC228C446C5DBB48F12";

  public BasicAuthentication(
    IOptionsMonitor<BasicAuthenticationOpt> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock
  ) : base(options,logger, encoder, clock) {
  
    _logger = logger.CreateLogger<BasicAuthentication>();
  }

  private string[]? GetAuth() {
    var auth = Request.Headers.Authorization.SingleOrDefault();
    if(auth is null) return null;

    var headSplit = auth.Split(" ");
    if(headSplit.Length != 2) return null;

    var creds = Encoding.UTF8.GetString(Convert.FromBase64String(headSplit[1]));

    var authSplit = creds.Split(":");
    if(authSplit.Length != 2) return null;

    return authSplit;
  }

  private string Hash(string pw) {
    var bytes = Encoding.UTF8.GetBytes(pw);

    var hash = SHA256.Create().ComputeHash(bytes);
    return Convert.ToHexString(hash);
  }

  protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
    await Task.Delay(1);

    var auth = GetAuth();
    if(auth is null) return AuthenticateResult.NoResult();

    if(auth[0] != "Shaco") return AuthenticateResult.NoResult();

    var hash = Hash(auth[1]);
    if(hash != _password) return AuthenticateResult.NoResult();
    
    var claimsIdentity = new ClaimsIdentity();
    claimsIdentity.AddClaim(new(ClaimTypes.Role,"admin"));

    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

    var ticket = new AuthenticationTicket(
      claimsPrincipal,
      Scheme.Name
    );
    
    return AuthenticateResult.Success(ticket);
  }
}