namespace BirchemmoWsServer.Server;
/// <summary>
/// To be used only in development. This object implicitly trusts the session token's profile, running no validation on the token itself.
/// </summary>
public class SessionTokenValidatorTrustAll : ISessionTokenValidator
{
  public User? GetUser(SessionToken token)
  {
    if (IsExpired(token)) return null;
    if (token.Profile is null) return new User();
    else return new User(token.Profile.UserId, token.Profile.Role);
  }

  private bool IsExpired(SessionToken token)
  {
    if (token.Profile is null) return true;
    return DateTime.Now >= token.Profile.Expires;
  }
}