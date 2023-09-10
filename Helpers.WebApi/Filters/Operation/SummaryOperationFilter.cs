using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Helpers.WebApi.Filters.Operation;

public class SummaryOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var methodSummary = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<EndpointSummaryAttribute>()
            .FirstOrDefault();

        if (methodSummary != null) operation.Summary = methodSummary.Summary;
    }
}