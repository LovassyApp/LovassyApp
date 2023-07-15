using Microsoft.AspNetCore.Authorization;

namespace Blueboard.Core.Auth.Policies.EmailConfirmed;

/// <summary>
///     The authorization policy attribute specifying whether the user's email should be verified or unverified (if
///     <c>shouldBeVerified</c> is set to false, verified users won't be allowed through) to access a specific
///     endpoint/controller. To allow both verified and unverified users, just use simply the
///     <see cref="AuthorizeAttribute" />.
/// </summary>
public class EmailVerifiedAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "EmailConfirmed";

    public EmailVerifiedAttribute(
        bool shouldBeVerified = true)
    {
        ShouldBeVerified = shouldBeVerified;
    }

    public bool ShouldBeVerified
    {
        get => bool.Parse(Policy!.Substring(PolicyPrefix.Length));
        set => Policy = $"{PolicyPrefix}{value.ToString()}";
    }
}