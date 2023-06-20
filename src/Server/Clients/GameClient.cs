using BirchemmoWsServer.Game;

namespace BirchemmoWsServer.Server;

public interface IGameClient
{
  Task Ping();
  Task ReturnPing();
  Task SendWorldState(ClientWorldState worldState);

  Task SendPawnsState(ClientPawnsState pawnsState);
}