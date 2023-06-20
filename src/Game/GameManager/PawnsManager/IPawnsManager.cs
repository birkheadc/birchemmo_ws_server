using BirchemmoWsServer.Server;

namespace BirchemmoWsServer.Game;

public interface IPawnsManager
{
  public List<Pawn> GetPawnsOwnedByUser(User user);
}