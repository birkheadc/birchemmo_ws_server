using BirchemmoWsServer.Server;

namespace BirchemmoWsServer.Game;

public interface IGameState
{
  public List<Pawn> GetPawnsOwnedByUser(User user);
}