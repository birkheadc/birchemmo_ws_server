using BirchemmoWsServer.Server;

namespace BirchemmoWsServer.Game;

public class PlayersManager : IPlayersManager
{
  public int GetAvailableNewPawnsForUser(User user)
  {
    if (user.Role >= Role.ADMIN) return 10;
    return 1;
  }
}