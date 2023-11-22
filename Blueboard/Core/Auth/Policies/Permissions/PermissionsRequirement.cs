using Microsoft.AspNetCore.Authorization;

namespace Blueboard.Core.Auth.Policies.Permissions;

public class PermissionsRequirement(string permissions) : IAuthorizationRequirement
{
    public string Permissions { get; } = permissions;
}