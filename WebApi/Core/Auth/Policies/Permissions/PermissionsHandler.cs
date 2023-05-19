using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Core.Auth.Policies.Permissions;

public class PermissionsHandler : AuthorizationHandler<PermissionsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionsRequirement requirement)
    {
        var userPermission = context.User.FindAll(AuthConstants.PermissionClaim).ToArray();

        if (userPermission.IsNullOrEmpty())
            return;

        var userPermissionsSet = new HashSet<string>(userPermission.Select(p => p.Value));

        foreach (var permission in requirement.Permissions.Split(","))
            if (!userPermissionsSet.Contains(permission))
                return;

        context.Succeed(requirement);
    }
}