using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Blueboard.Core.Auth.Filters.Operation;

public class EmailVerifiedOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var emailVerified = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<EmailVerifiedAttribute>()
            .FirstOrDefault() ?? context.MethodInfo.DeclaringType!
            .GetCustomAttributes(true).OfType<EmailVerifiedAttribute>()
            .FirstOrDefault();

        if (context.MethodInfo.GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>().Any())
            emailVerified = null;

        if (emailVerified is { ShouldBeVerified: true })
        {
            if (!string.IsNullOrWhiteSpace(operation.Description))
                operation.Description += "<br>";
            operation.Description += "<b>Requires verified email</b>";
        }

        if (emailVerified is { ShouldBeVerified: false })
        {
            if (!string.IsNullOrWhiteSpace(operation.Description))
                operation.Description += "<br>";
            operation.Description += "<b>Requires unverified email</b>";
        }
    }
}