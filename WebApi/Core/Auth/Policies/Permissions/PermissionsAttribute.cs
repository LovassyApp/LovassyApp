using Microsoft.AspNetCore.Authorization;
using WebApi.Core.Auth.Interfaces;

namespace WebApi.Core.Auth.Policies.Permissions;

public class PermissionsAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "Permissions";

    public PermissionsAttribute(params Type[] permissions)
    {
        if (permissions.Any(p => !typeof(IPermission).IsAssignableFrom(p)))
            throw new ArgumentException("Type must implement IPermission", nameof(permissions));

        if (PermissionLookup.NamesToPermissions == null || PermissionLookup.PermissionsToNames == null)
            throw new InvalidOperationException("Permissions are not loaded yet");

        Permissions = permissions.Select(p => PermissionLookup.PermissionsToNames[p])
            .Aggregate((a, b) => $"{a},{b}");
    }

    public string Permissions
    {
        get => Policy.Substring(PolicyPrefix.Length);
        set => Policy = $"{PolicyPrefix}{value}";
    }
}