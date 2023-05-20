using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using WebApi.Core.Auth.Services.Options;
using WebApi.Core.Auth.Utils;

namespace WebApi.Core.Auth.Policies.Permissions;

public class PermissionsHandler : AuthorizationHandler<PermissionsRequirement>
{
    private readonly PermissionsOptions _permissionsOptions;

    public PermissionsHandler(IOptions<PermissionsOptions> permissionsOptions)
    {
        _permissionsOptions = permissionsOptions.Value;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionsRequirement requirement)
    {
        if (PermissionUtils.CheckPermissions(context.User, requirement.Permissions.Split(",")))
            context.Succeed(requirement);
    }
}