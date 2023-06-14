using BirchemmoWsServer.Game;

namespace BirchemmoWsServer.Server;

public interface IGameClient
{
  Task RequestSessionToken();
  Task ConfirmSessionToken(bool isValid);
  Task SendWorld(int[] world);

  Task SendOwnedPawns(List<Pawn> pawns, int? currentPawn);
}