using Microsoft.AspNetCore.Authorization;

namespace WebApi.Core.Auth.Policies.EmailConfirmed;

public class EmailVerifiedRequirement : IAuthorizationRequirement
{
    public EmailVerifiedRequirement(EmailVerifiedPrecondition emailVerifiedPrecondition)
    {
        EmailVerifiedPrecondition = emailVerifiedPrecondition;
    }

    public EmailVerifiedPrecondition EmailVerifiedPrecondition { get; }
}