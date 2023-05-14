using Microsoft.AspNetCore.Authorization;

namespace WebApi.Core.Auth.Policies.EmailConfirmed;

public class EmailVerifiedRequirement : IAuthorizationRequirement
{
    public EmailVerifiedRequirement(bool shouldBeVerified)
    {
        ShouldBeVerified = shouldBeVerified;
    }

    public bool ShouldBeVerified { get; }
}