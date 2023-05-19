using Microsoft.AspNetCore.Authorization;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Core.Auth.Policies.Permissions;

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

        if (policyName.StartsWith(PermissionsAttribute.PolicyPrefix))
        {
            var policy = new AuthorizationPolicyBuilder(AuthConstants.TokenScheme);
            policy.AddRequirements(
                new PermissionsRequirement(policyName.Substring(PermissionsAttribute.PolicyPrefix.Length)));
            return policy.Build();
        }

        return null;
    }

    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        // Do not set any schemes here!!! It will result in the default policy accepting it even if it's not in the AuthSchemes of the authorize attribute
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    }

    public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return null;
    }
}