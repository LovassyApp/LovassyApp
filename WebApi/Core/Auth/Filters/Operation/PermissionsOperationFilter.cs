using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi.Core.Auth.Policies.Permissions;

namespace WebApi.Core.Auth.Filters.Operation;

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
                operation.Description += "<br>";
            operation.Description +=
                $"<b>Requires one of the following permissions</b>: {string.Join(", ", permissions.Permissions.Split(","))}";
        }
    }
}