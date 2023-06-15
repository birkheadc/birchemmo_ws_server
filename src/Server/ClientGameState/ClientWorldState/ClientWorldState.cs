using BirchemmoWsServer.Game;
using BirchemmoWsServer.Server;

public record ClientWorldState
{
  public List<Chunk> Chunks { get; set; } = new List<Chunk>();
}