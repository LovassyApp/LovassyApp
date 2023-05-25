namespace WebApi.Core.Auth;

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
    public const string UserGroupClaim = "UserGroup";

    public const int DefaultUserGroupID = 1;
}