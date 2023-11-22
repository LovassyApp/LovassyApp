using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Blueboard.Core.Auth.Policies;

/// <summary>
///     This class solves the issue of requirements failing if they are evaluated before RequireAuthenticatedUser.
///     It is unpredictable asp.net core authorization whether it checks requirements before checking if the user is
///     authenticated and it makes no sense. (With this RequireAuthenticatedUser is not needed when defining policies)
/// </summary>
public class ChallengeUnauthenticatedPolicyEvaluator(PolicyEvaluator defaultEvaluator) : IPolicyEvaluator
{
    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        return defaultEvaluator.AuthenticateAsync(policy, context);
    }

    public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
        AuthenticateResult authenticationResult, HttpContext context,
        object? resource)
    {
        if (!authenticationResult.Succeeded)
            return Task.FromResult(PolicyAuthorizationResult.Challenge());

        return defaultEvaluator.AuthorizeAsync(policy, authenticationResult, context, resource);
    }
}