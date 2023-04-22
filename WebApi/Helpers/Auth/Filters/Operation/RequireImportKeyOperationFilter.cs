using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Helpers.Auth.Filters.Operation;

public class RequireImportKeyOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var requiresImportKey = context.MethodInfo
                                    .GetCustomAttributes(true)
                                    .OfType<AuthorizeAttribute>()
                                    .Any(a => a.AuthenticationSchemes != null && a.AuthenticationSchemes.Split(",")
                                        .Contains(AuthConstants.ImportKeyScheme)) ||
                                (context.MethodInfo.DeclaringType!
                                    .GetCustomAttributes(true).OfType<AuthorizeAttribute>()
                                    .Any(a => a.AuthenticationSchemes != null && a.AuthenticationSchemes.Split(",")
                                        .Contains(AuthConstants.ImportKeyScheme)) && !context.MethodInfo
                                    .GetCustomAttributes(true)
                                    .OfType<AllowAnonymousAttribute>().Any());

        if (requiresImportKey)
            operation.Parameters.Add(new OpenApiParameter
            {
                Required = true, Name = "X-Authorization", In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "string" }
            });
    }
}