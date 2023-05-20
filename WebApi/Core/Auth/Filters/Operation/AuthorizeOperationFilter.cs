using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Core.Auth.Filters.Operation;

public class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authorize = context.MethodInfo
                            .GetCustomAttributes(true)
                            .OfType<AuthorizeAttribute>()
                            .FirstOrDefault() ??
                        context.MethodInfo.DeclaringType!
                            .GetCustomAttributes(true).OfType<AuthorizeAttribute>()
                            .FirstOrDefault();

        if (context.MethodInfo.GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>().Any())
            authorize = null;

        if (authorize != null)
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            if (string.IsNullOrEmpty(authorize.AuthenticationSchemes) ||
                authorize.AuthenticationSchemes.Split(",").Contains(AuthConstants.TokenScheme))
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

            if (!string.IsNullOrEmpty(authorize.AuthenticationSchemes) &&
                authorize.AuthenticationSchemes.Contains(AuthConstants.ImportKeyScheme))
                operation.Security.Add(new OpenApiSecurityRequirement
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
                });
        }
    }
}