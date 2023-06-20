using BirchemmoWsServer.Game;

namespace BirchemmoWsServer.Server;
/// <summary>
/// Maps to the part of a client's game state that manages that user's pawns.
/// </summary>
public record ClientPawnsState
{
  public List<Pawn> Pawns { get; set; }
  public int AvailableNew { get; set; }

  public ClientPawnsState(List<Pawn> pawns, int availableNew)
  {
    Pawns = pawns;
    AvailableNew = availableNew;
  }
}