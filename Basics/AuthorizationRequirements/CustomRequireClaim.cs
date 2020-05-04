using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Basics.AuthorizationRequirements
{
  public class CustomRequireClaim : IAuthorizationRequirement
  {
    public CustomRequireClaim(string claimType)
    {
      ClaimType = claimType;
    }

    public string ClaimType { get; }
  }

  public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
  {
    //public CustomRequireClaimHandler()
    //{
    //  //services / databases get injected here - would use scoped on startup in that case
    //}

    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      CustomRequireClaim requirement)
    {
      var hasclaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);

      if (hasclaim)
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }

  public static class AuthorizationPolicyBuilderExtensions
  {
    public static AuthorizationPolicyBuilder RequireCustomClaim(
      this AuthorizationPolicyBuilder builder, string claimtype)
    {
      builder.AddRequirements(new CustomRequireClaim(claimtype));
      return builder;
    }
  }
}