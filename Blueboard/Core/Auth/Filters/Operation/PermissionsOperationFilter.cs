using Blueboard.Core.Auth.Policies.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Blueboard.Core.Auth.Filters.Operation;

public class PermissionsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var permissions = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<PermissionsAttribute>()
            .FirstOrDefault() ?? context.MethodInfo.DeclaringType!
            .GetCustomAttributes(true).OfType<PermissionsAttribute>()
            .FirstOrDefault();

        if (context.MethodInfo.GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>().Any())
            permissions = null;

        if (permissions != null)
        {
            if (!string.IsNullOrWhiteSpace(operation.Description))
                operation.Description += "; ";
            operation.Description +=
                $"Requires one of the following permissions: {string.Join(", ", permissions.Permissions.Split(","))}";
        }
    }
}