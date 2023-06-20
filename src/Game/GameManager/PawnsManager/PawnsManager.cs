using BirchemmoWsServer.Server;

namespace BirchemmoWsServer.Game;

public class PawnsManager : IPawnsManager
{
  private readonly Dictionary<Guid, List<Pawn>> UserPawns = new();

  public List<Pawn> GetPawnsOwnedByUser(User user)
  {
    List<Pawn> pawns = new();
    return pawns;
  }
}