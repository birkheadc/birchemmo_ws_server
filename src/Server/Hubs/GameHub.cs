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

    User? user = sessionTokenValidator.GetUser(token);

    if (user is null) return Task.Run(() => {
      Clients.Caller.ConfirmSessionToken(false);
    });
    
    AddUserToContext(user, Context);
    return Task.Run(() => {
      Clients.Caller.ConfirmSessionToken(true);
    });
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

    ClientPawnsState pawnsState = new()
    {
      Pawns = pawns,
      Current = 0,
      AvailableNew = user?.Role == Role.SUPER_ADMIN ? 10 : 1
    };

    await Clients.Caller.SendPawnsState(pawnsState);
  }

  private void AddUserToContext(User user, HubCallerContext context)
  {
    Console.WriteLine("Adding user to context:");
    Console.WriteLine(user.ToString());
    context.Items.Add("user", user);
  }

  private User? GetUserFromContext(HubCallerContext context)
  {
    context.Items.TryGetValue("user", out var _user);
    Console.WriteLine("Found user in context:");
    User? user = _user as User;
    Console.WriteLine(user is null ? "null" : (user as User).ToString());
    return user;
  }
}