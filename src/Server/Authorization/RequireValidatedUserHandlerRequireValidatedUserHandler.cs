using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BirchemmoWsServer.Server;

public class RequireValidatedUserHandler : AuthorizationHandler<RequireValidatedUserRequirement>
{
  private readonly IHttpContextAccessor httpContextAccessor;

  public RequireValidatedUserHandler(IHttpContextAccessor httpContextAccessor)
  {
    this.httpContextAccessor = httpContextAccessor;
  }

  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireValidatedUserRequirement requirement)
  {
    Console.WriteLine("Authorizing...");

    if (IsNegotiating())
    {
      Console.WriteLine("Negotiating allow anonymous.");
      context.Succeed(requirement);
      return Task.CompletedTask;
    }
    
    if (Enum.TryParse<Role>(context.User.FindFirstValue(ClaimTypes.Role), out var role))
    {
      Console.WriteLine(role);
      if (role >= Role.VALIDATED_USER)
      {
        context.Succeed(requirement);
        return Task.CompletedTask;
      }
      context.Fail(new AuthorizationFailureReason(this, "Role was not high enough."));
      return Task.CompletedTask;
    }
    context.Fail(new AuthorizationFailureReason(this, "Role claim was not found or was not valid."));
    Console.WriteLine("Failed? " + context.HasFailed);
    if (context.HasFailed)
    {
      foreach (var reason in context.FailureReasons)
      {
        Console.WriteLine(reason.Message);
      }
    }
    return Task.CompletedTask;
  }

  private bool IsNegotiating()
  {
    return httpContextAccessor.HttpContext?.Request.Path.ToString().Contains("/negotiate") ?? false;
  }
}