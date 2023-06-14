namespace BirchemmoWsServer.Server;

public interface ISessionTokenValidator
{
  public bool IsValid(SessionToken token);
}