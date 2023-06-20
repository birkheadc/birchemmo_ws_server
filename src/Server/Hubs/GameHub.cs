using System.Security.Claims;
using BirchemmoWsServer.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BirchemmoWsServer.Server;

// [Authorize(Policy = "RequireValidatedUser")]
public class GameHub : Hub<IGameClient>
{
  private readonly IGameManager gameManager;

  public GameHub(IGameManager gameManager)
  {
    this.gameManager = gameManager;
  }

  public async Task Ping()
  {
    User? user = GetUserFromContext(Context);
    Console.WriteLine($"Received a ping from {user?.ToString()}.");
    await Clients.Caller.ReturnPing();
  }

  public Task ReturnPing()
  {
    Console.WriteLine("Client returned ping.");
    return Task.CompletedTask;
  }

  public async Task RequestWorldState()
  {
    Console.WriteLine("World requested from client...");
    await Clients.Caller.SendWorldState(new ClientWorldState());
  }

  public async Task RequestPawnsState()
  {
    List<Pawn> pawns = new();
    pawns.Add(new Pawn(title: "Pawn 1"));
    pawns.Add(new Pawn(title: "Pawn 1"));

    User? user = GetUserFromContext(Context);
    if (user is null) return;

    ClientPawnsState pawnsState = gameManager.GetPawnsStateForUser(user);

    await Clients.Caller.SendPawnsState(pawnsState);
  }

  private User? GetUserFromContext(HubCallerContext context)
  {
    Console.WriteLine("");
    Console.WriteLine("Getting User From Context...");
    Console.WriteLine("All user's claims: ");
    List<Claim> claims = Context.User?.Claims.ToList() ?? new List<Claim>();
    foreach (Claim claim in claims)
    {
      Console.WriteLine("----------------------------------------");
      Console.WriteLine($"Claim.Type | {claim.Type}");
      Console.WriteLine($"Claim.Value | {claim.Value}");
      Console.WriteLine($"Claim.ValueType | {claim.ValueType}");
      Console.WriteLine("----------------------------------------");
    }
    Console.WriteLine($"Is user authenticated? | {Context.User?.Identity?.IsAuthenticated}");
    Console.WriteLine("");
    return Context.User is null ? null : new User(Context.User);
  }
}