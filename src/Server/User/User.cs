namespace BirchemmoWsServer.Server;

public class User
{
  public Guid Id { get; set; }
  public Role Role { get; set; }

  public User()
  {
    Id = Guid.NewGuid();
    Role = Role.VISITOR;
  }

  public User(string id, Role role)
  {
    Id = Guid.Parse(id);
    Role = role;
  }

  public override string ToString()
  {
    return $"User: {Id}\n{Role}";
  }
}