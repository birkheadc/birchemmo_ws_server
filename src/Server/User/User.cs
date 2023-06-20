using System.Security.Claims;

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

  public User(Guid id, Role role)
  {
    Id = id;
    Role = role;
  }

  public User(ClaimsPrincipal claims)
  {
    Id = GetIdFromClaims(claims);
    Role = GetRoleFromClaims(claims);
  }

  private Guid GetIdFromClaims(ClaimsPrincipal claims)
  {
    try
    {
      return Guid.Parse(claims.FindFirstValue(ClaimTypes.NameIdentifier));
    }
    catch
    {
      return Guid.Empty;
    }
  }

  private Role GetRoleFromClaims(ClaimsPrincipal claims)
  {
    string? claim = claims.FindFirstValue(ClaimTypes.Role);
    if (claim is null) return Role.VISITOR;
    if (Enum.TryParse(claim, out Role role)) return role;
    return Role.VISITOR;
  }

  public override string ToString()
  {
    return $"User: {Id}\n{Role}";
  }
}