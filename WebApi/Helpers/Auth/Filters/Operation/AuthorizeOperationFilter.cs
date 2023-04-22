using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Helpers.Auth.Filters.Operation;

public class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo
                               .GetCustomAttributes(true)
                               .OfType<AuthorizeAttribute>()
                               .Any() ||
                           (context.MethodInfo.DeclaringType!
                               .GetCustomAttributes(true).OfType<AuthorizeAttribute>()
                               .Any() && !context.MethodInfo.GetCustomAttributes(true)
                               .OfType<AllowAnonymousAttribute>().Any());

        if (hasAuthorize)
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
        }
    }
}