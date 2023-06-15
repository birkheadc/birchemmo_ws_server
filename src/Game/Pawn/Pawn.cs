namespace BirchemmoWsServer.Game;

public class Pawn
{
  public Guid Id { get; set; }
  public string Title { get; set; }

  public Pawn(Guid id, string title)
  {
    Id = id;
    Title = title;
  }

  public Pawn(string title)
  {
    Id = Guid.NewGuid();
    Title = title;
  }
}