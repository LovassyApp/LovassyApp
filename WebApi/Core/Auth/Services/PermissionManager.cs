using System.Security.Claims;
using WebApi.Core.Auth.Interfaces;
using WebApi.Core.Auth.Utils;

namespace WebApi.Core.Auth.Services;

public class PermissionManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private ClaimsPrincipal?
        _claimsPrincipal; //Working with the claims principal rather than the user just makes more sense here

    public PermissionManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    ///     Checks if the current user has the given permission.
    /// </summary>
    /// <param name="permission">The permission to check for.</param>
    /// <returns>Whether the user has the permission or not.</returns>
    public bool CheckPermission(Type permission)
    {
        if (_claimsPrincipal == null)
            Init();

        if (!typeof(IPermission).IsAssignableFrom(permission))
            throw new ArgumentException("Type must implement IPermission", nameof(permission));

        return PermissionUtils.CheckPermissions(_claimsPrincipal!,
            new[] { PermissionUtils.PermissionTypesToNames![permission] });
    }

    /// <summary>
    ///     Checks if the current user is a super user.
    /// </summary>
    /// <returns>Whether the current user is a super user or not.</returns>
    public bool CheckSuperUser()
    {
        if (_claimsPrincipal == null)
            Init();

        return PermissionUtils.CheckSuperUser(_claimsPrincipal!);
    }

    private void Init()
    {
        if (_httpContextAccessor.HttpContext!.User.Identity?.IsAuthenticated == false)
            throw new InvalidOperationException("User (ClaimsPrincipal) is not authenticated");

        _claimsPrincipal = _httpContextAccessor.HttpContext.User;
    }
}