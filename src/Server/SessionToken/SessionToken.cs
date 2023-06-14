namespace BirchemmoWsServer.Server;

public record SessionToken
{
  public string Token { get; set; } = "";
  public SessionTokenProfile? Profile { get; set; }

  public override string ToString()
  {
    return $"Token: {Token}\n{Profile?.ToString()}";
  }
}