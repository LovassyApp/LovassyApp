using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Blueboard.Core.Auth.Policies.EmailConfirmed;

public class EmailVerifiedHandler : AuthorizationHandler<EmailVerifiedRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        EmailVerifiedRequirement requirement)
    {
        var emailConfirmed = context.User.FindFirstValue(AuthConstants.EmailVerifiedClaim);

        if (emailConfirmed == null)
            return;

        if (requirement.ShouldBeVerified == bool.Parse(emailConfirmed))
            context.Succeed(requirement);
    }
}