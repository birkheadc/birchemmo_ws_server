using BirchemmoWsServer.Server;

namespace BirchemmoWsServer.Game;

public class GameManager : IGameManager
{
  private readonly IPawnsManager pawnsManager;
  private readonly IWorldManager worldManager;
  private readonly IPlayersManager playersManager;

  public GameManager(IPawnsManager pawnsManager, IWorldManager worldManager, IPlayersManager playersManager)
  {
    this.pawnsManager = pawnsManager;
    this.worldManager = worldManager;
    this.playersManager = playersManager;
  }

  public ClientPawnsState GetPawnsStateForUser(User user)
  {
    List<Pawn> pawns = pawnsManager.GetPawnsOwnedByUser(user);
    int availableNew = playersManager.GetAvailableNewPawnsForUser(user);

    return new ClientPawnsState(pawns, availableNew);
  }
}