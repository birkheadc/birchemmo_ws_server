using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BirchemmoWsServer.Server;

public class RequireValidatedUserHandler : AuthorizationHandler<RequreValidatedUserRequirement>
{
  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequreValidatedUserRequirement requirement)
  {
    Console.WriteLine("Authorizing...");
    // context.Succeed(requirement);
    // return Task.CompletedTask;
    Console.WriteLine(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
    if (Enum.TryParse<Role>(context.User.FindFirstValue(ClaimTypes.Role), out var role))
    {
      Console.WriteLine(role);
      if (role >= Role.VALIDATED_USER) context.Succeed(requirement);
      else context.Fail(new AuthorizationFailureReason(this, "Role was not high enough."));
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
}