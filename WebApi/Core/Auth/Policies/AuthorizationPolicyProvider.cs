using Microsoft.AspNetCore.Authorization;
using WebApi.Core.Auth.Policies.EmailConfirmed;

namespace WebApi.Core.Auth.Policies;

public class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(EmailVerifiedAuthorizeAttribute.PolicyPrefix) && Enum.TryParse(
                policyName.Substring(EmailVerifiedAuthorizeAttribute.PolicyPrefix.Length),
                out EmailVerifiedPrecondition precondition))
        {
            var policy = new AuthorizationPolicyBuilder(AuthConstants.TokenScheme);
            policy.AddRequirements(new EmailVerifiedRequirement(precondition));
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