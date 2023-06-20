using BirchemmoWsServer.Server;

namespace BirchemmoWsServer.Game;

public interface IGameManager
{
  public ClientPawnsState GetPawnsStateForUser(User user);
}