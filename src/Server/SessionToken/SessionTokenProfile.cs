namespace BirchemmoWsServer.Server;

public record SessionTokenProfile
{
  public DateTime Expires { get; set; }
  public string UserId { get; set; } = "";
  public Role Role { get; set; }

  public override string ToString()
  {
    return $"Expires: {Expires.ToLocalTime()}\nUserId: {UserId}\nRole: {Role.ToString()}";
  }
}