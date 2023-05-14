using Microsoft.AspNetCore.Authorization;
using WebApi.Core.Auth.Policies.EmailConfirmed;

namespace WebApi.Core.Auth.Policies;

/// <summary>
///     The custom authorization policy provider for the application. It is used to handle all policies.
/// </summary>
public class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(EmailVerifiedAttribute.PolicyPrefix) && bool.TryParse(
                policyName.Substring(EmailVerifiedAttribute.PolicyPrefix.Length),
                out var shouldBeVerified))
        {
            var policy = new AuthorizationPolicyBuilder(AuthConstants.TokenScheme);
            policy.AddRequirements(new EmailVerifiedRequirement(shouldBeVerified));
            return policy.Build();
        }

        return null;
    }

    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(AuthConstants.TokenScheme, AuthConstants.ImportKeyScheme)
            .RequireAuthenticatedUser().Build();
    }

    public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return null;
    }
}