using System.Security.Claims;

namespace Blueboard.Core.Auth;

/// <summary>
///     The static class containing all the constants related the authentication core feature.
/// </summary>
public static class AuthConstants
{
    public const string TokenScheme = "Token";
    public const string ImportKeyScheme = "ImportKey";

    public const string RefreshCookieKey = "BlueboardRefresh";

    public const string EmailVerifiedClaim = "EmailVerified";
    public const string PermissionClaim = "Permission";
    public const string SuperUserClaim = "SuperUser";
    public const string FeatureUserClaim = ClaimTypes.Email;
    public const string FeatureGroupClaim = "UserGroup";

    public const int DefaultUserGroupID = 1;
}