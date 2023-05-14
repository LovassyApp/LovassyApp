using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Core.Auth.Policies.EmailConfirmed;

public class EmailVerifiedHandler : AuthorizationHandler<EmailVerifiedRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        EmailVerifiedRequirement requirement)
    {
        var emailConfirmed = context.User.FindFirstValue(AuthConstants.EmailVerifiedClaim);

        if (emailConfirmed == null)
            return;

        switch (requirement.EmailVerifiedPrecondition)
        {
            case EmailVerifiedPrecondition.Any:
                context.Succeed(requirement);
                break;
            case EmailVerifiedPrecondition.Confirmed:
                if (emailConfirmed == bool.TrueString)
                    context.Succeed(requirement);
                break;
            case EmailVerifiedPrecondition.Unconfirmed:
                if (emailConfirmed == bool.FalseString)
                    context.Succeed(requirement);
                break;
        }
    }
}