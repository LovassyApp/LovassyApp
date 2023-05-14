using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace WebApi.Core.Auth.Policies;

public class ChallengeUnauthenticatedPolicyEvaluator : IPolicyEvaluator
{
    private readonly PolicyEvaluator _defaultEvaluator;

    public ChallengeUnauthenticatedPolicyEvaluator(PolicyEvaluator defaultEvaluator)
    {
        _defaultEvaluator = defaultEvaluator;
    }


    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        return _defaultEvaluator.AuthenticateAsync(policy, context);
    }


    public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
        AuthenticateResult authenticationResult, HttpContext context,
        object? resource)
    {
        if (!authenticationResult.Succeeded)
            return Task.FromResult(PolicyAuthorizationResult.Challenge());

        return _defaultEvaluator.AuthorizeAsync(policy, authenticationResult, context, resource);
    }
}