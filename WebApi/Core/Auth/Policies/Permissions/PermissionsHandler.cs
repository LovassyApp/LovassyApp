using Microsoft.AspNetCore.Authorization;
using WebApi.Core.Auth.Utils;

namespace WebApi.Core.Auth.Policies.Permissions;

public class PermissionsHandler : AuthorizationHandler<PermissionsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionsRequirement requirement)
    {
        if (PermissionUtils.CheckPermissions(context.User, requirement.Permissions.Split(",")))
            context.Succeed(requirement);
    }
}