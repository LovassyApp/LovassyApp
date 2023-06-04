using Microsoft.AspNetCore.Authorization;

namespace Blueboard.Core.Auth.Policies.EmailConfirmed;

public class EmailVerifiedRequirement : IAuthorizationRequirement
{
    public EmailVerifiedRequirement(bool shouldBeVerified)
    {
        ShouldBeVerified = shouldBeVerified;
    }

    public bool ShouldBeVerified { get; }
}