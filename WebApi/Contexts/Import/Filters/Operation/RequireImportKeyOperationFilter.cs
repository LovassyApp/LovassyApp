using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi.Contexts.Import.Filters.Action;

namespace WebApi.Contexts.Import.Filters.Operation;

public class RequireImportKeyOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var requiresImportKey = context.MethodInfo
                                    .GetCustomAttributes(true)
                                    .OfType<ServiceFilterAttribute>()
                                    .Any(f => f.ServiceType == typeof(RequireImportKeyFilter)) ||
                                context.MethodInfo.DeclaringType!
                                    .GetCustomAttributes(true).OfType<ServiceFilterAttribute>()
                                    .Any(f => f.ServiceType == typeof(RequireImportKeyFilter));

        if (requiresImportKey)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Required = true, Name = "X-Authorization", In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(),
                new OpenApiResponse { Description = "Unauthorized" });
        }
    }
}