using Blueboard.Core.Auth.Interfaces;
using Blueboard.Core.Auth.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Blueboard.Core.Auth.Policies.Permissions;

/// <summary>
///     The authorization policy attribute specifying certain permission, of which the user must have at least one to
///     access a specific endpoint/controller.
/// </summary>
public class PermissionsAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "Permissions";

    public PermissionsAttribute(params Type[] permissions)
    {
        if (permissions.Any(p => !typeof(IPermission).IsAssignableFrom(p)))
            throw new ArgumentException("Type must implement IPermission", nameof(permissions));

        if (PermissionUtils.PermissionTypesToNames == null)
            throw new InvalidOperationException("Permissions are not loaded yet");

        Permissions = permissions.Select(p => PermissionUtils.PermissionTypesToNames[p])
            .Aggregate((a, b) => $"{a},{b}");
    }

    public string Permissions
    {
        get => Policy!.Substring(PolicyPrefix.Length);
        set => Policy = $"{PolicyPrefix}{value}";
    }
}