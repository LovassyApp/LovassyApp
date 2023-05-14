using Microsoft.AspNetCore.Authorization;

namespace WebApi.Core.Auth.Policies.EmailConfirmed;

public class EmailVerifiedAuthorizeAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "EmailConfirmed";

    public EmailVerifiedAuthorizeAttribute(
        EmailVerifiedPrecondition emailVerifiedPrecondition = EmailVerifiedPrecondition.Confirmed)
    {
        EmailVerifiedPrecondition = emailVerifiedPrecondition;
    }

    public EmailVerifiedPrecondition EmailVerifiedPrecondition
    {
        get => (EmailVerifiedPrecondition)int.Parse(Policy.Substring(PolicyPrefix.Length));
        set => Policy = $"{PolicyPrefix}{value}";
    }
}