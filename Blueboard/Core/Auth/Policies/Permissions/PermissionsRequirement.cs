using Microsoft.AspNetCore.Authorization;

namespace Blueboard.Core.Auth.Policies.Permissions;

public class PermissionsRequirement : IAuthorizationRequirement
{
    public PermissionsRequirement(string permissions)
    {
        Permissions = permissions;
    }

    public string Permissions { get; }
}