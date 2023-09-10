using Microsoft.FeatureManagement.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Helpers.WebApi.Filters.Operation;

public class FeatureGateOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var methodFeatureGate = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<FeatureGateAttribute>()
            .FirstOrDefault();

        if (methodFeatureGate != null)
        {
            if (!string.IsNullOrWhiteSpace(operation.Description))
                operation.Description += "; ";
            operation.Description +=
                $"Requires the following features to be enabled: {string.Join(", ", methodFeatureGate.Features)}";
        }

        var classFeatureGate = context.MethodInfo.DeclaringType!
            .GetCustomAttributes(true).OfType<FeatureGateAttribute>()
            .FirstOrDefault();

        if (classFeatureGate != null)
        {
            if (!string.IsNullOrWhiteSpace(operation.Description))
                operation.Description += "; ";
            operation.Description +=
                $"Requires the following features to be enabled: {string.Join(", ", classFeatureGate.Features)}";
        }
    }
}