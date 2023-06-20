using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BirchemmoWsServer.Server;

public class SignalRAuthenticationHandler : AuthenticationHandler<SignalRAuthenticationOptions>
{
  public SignalRAuthenticationHandler(IOptionsMonitor<SignalRAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
  {

  }

  protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    Console.WriteLine("Attempting to authenticate...");
    // Get the session token from the request (e.g., query string, header)
    string? sessionToken = GetSessionTokenFromRequest();
    if (sessionToken is null) return AuthenticateResult.Fail("Session token not found");

    ClaimsPrincipal? principal = CreateClaimsPrincipalFromToken(sessionToken);
    if (principal is null) return AuthenticateResult.Fail("Session token was invalid.");

    // Save the claims principal in the context
    Context.User = principal;

    // Create an authentication ticket
    AuthenticationTicket ticket = new(principal, Scheme.Name);

    // Return the authentication result
    Console.WriteLine("Authentication Successful.");
    return await Task.FromResult(AuthenticateResult.Success(ticket));
  }

  private string? GetSessionTokenFromRequest()
  {
    string tokenString = Request.Query["access_token"];
    Console.WriteLine("Found token " + tokenString);
    return tokenString;
  }
  
  private ClaimsPrincipal? CreateClaimsPrincipalFromToken(string sessionToken)
  {
    try
    {
      TokenValidationParameters validationParameters = new()
      {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretsecretsecretsecretsecretsecret")),
        ValidateIssuerSigningKey = true
      };

      JwtSecurityTokenHandler handler = new();
      ClaimsPrincipal claimsPrincipal = handler.ValidateToken(sessionToken, validationParameters, out _);
      return claimsPrincipal;
    }
    catch (Exception e)
    {
      Console.WriteLine(e.Message);
      return null;
    }
    
  }
}