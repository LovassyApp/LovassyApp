using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Contexts.Import.Services;

namespace WebApi.Contexts.Import.Filters.Action;

public class RequireImportKeyFilter : IAsyncActionFilter
{
    private readonly ImportKeyService _importKeyService;

    public RequireImportKeyFilter(ImportKeyService importKeyService)
    {
        _importKeyService = importKeyService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var importKey = context.HttpContext.Request.Headers.FirstOrDefault(q => q.Key == "X-Authorization").Value
            .FirstOrDefault();

        if (importKey == null || !await _importKeyService.IsImportKeyValidAsync(importKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}