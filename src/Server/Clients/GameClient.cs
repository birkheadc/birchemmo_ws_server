using BirchemmoWsServer.Game;

namespace BirchemmoWsServer.Server;

public interface IGameClient
{
  Task RequestSessionToken();
  Task ConfirmSessionToken(bool isValid);
  Task SendWorldState(ClientWorldState worldState);

  Task SendPawnsState(ClientPawnsState pawnsState);
}