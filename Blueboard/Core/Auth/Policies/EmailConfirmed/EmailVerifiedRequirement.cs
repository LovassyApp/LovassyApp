using Microsoft.AspNetCore.Authorization;

namespace Blueboard.Core.Auth.Policies.EmailConfirmed;

public class EmailVerifiedRequirement(bool shouldBeVerified) : IAuthorizationRequirement
{
    public bool ShouldBeVerified { get; } = shouldBeVerified;
}