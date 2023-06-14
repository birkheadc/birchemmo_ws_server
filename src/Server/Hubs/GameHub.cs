using System.Security.Claims;
using BirchemmoWsServer.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BirchemmoWsServer.Server;

[Authorize(Policy = "Test")]
public class GameHub : Hub<IGameClient>
{

  private readonly ISessionTokenValidator sessionTokenValidator;

  public GameHub(ISessionTokenValidator sessionTokenValidator)
  {
    this.sessionTokenValidator = sessionTokenValidator;
  }

  public override async Task OnConnectedAsync()
  {
    await Clients.Caller.RequestSessionToken();
  }

  public Task SendSessionToken(SessionToken token)
  {
    Console.WriteLine("We received a session token!");
    Console.WriteLine(token.ToString());
    bool isValid = sessionTokenValidator.IsValid(token);
    return Task.Run(() => {
      Clients.Caller.ConfirmSessionToken(isValid);
    });
  }
  public async Task RequestWorld()
  {
    Console.WriteLine("World requested from client...");
    int[] world = new int[]{ 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
    await Clients.Caller.SendWorld(world);
  }

  public async Task RequestOwnedPawns()
  {
    var user = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    Console.WriteLine("User requesting owned pawns");
    Console.WriteLine(user);

    List<Pawn> pawns = new();
    pawns.Add(new Pawn()
    {
      Title = "Pawn 1"
    });
    pawns.Add(new Pawn()
    {
      Title = "Pawn 2"
    });
    await Clients.Caller.SendOwnedPawns(pawns, 0);
  }
}