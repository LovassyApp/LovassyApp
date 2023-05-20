using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Core.Auth.Filters.Operation;

public class ImportKeyOperationFilter : IOperationFilter
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
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ImportKey"
                            }
                        },
                        new string[] { }
                    }
                }
            };
    }
}