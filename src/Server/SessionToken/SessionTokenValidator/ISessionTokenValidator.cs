namespace BirchemmoWsServer.Server;

public interface ISessionTokenValidator
{
  public User? GetUser(SessionToken token);
}