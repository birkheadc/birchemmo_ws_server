using BirchemmoWsServer.Server;

namespace BirchemmoWsServer.Game;

public interface IPlayersManager
{
  public int GetAvailableNewPawnsForUser(User user);
}