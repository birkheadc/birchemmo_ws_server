namespace BirchemmoWsServer.Server;
/// <summary>
/// To be used only in development. This object implicitly trusts the session token's profile, running no validation on the token itself.
/// </summary>
public class SessionTokenValidatorTrustAll : ISessionTokenValidator
{
  public bool IsValid(SessionToken token)
  {
    return !IsExpired(token);
  }

  private bool IsExpired(SessionToken token)
  {
    if (token.Profile is null) return true;
    return DateTime.Now >= token.Profile.Expires;
  }
}