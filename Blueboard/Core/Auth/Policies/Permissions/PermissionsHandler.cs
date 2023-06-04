using Blueboard.Core.Auth.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Blueboard.Core.Auth.Policies.Permissions;

public class PermissionsHandler : AuthorizationHandler<PermissionsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionsRequirement requirement)
    {
        if (PermissionUtils.CheckPermissions(context.User, requirement.Permissions.Split(",")))
            context.Succeed(requirement);
    }
}