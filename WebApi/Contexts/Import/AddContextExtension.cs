using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi.Contexts.Import.Filters.Action;
using WebApi.Contexts.Import.Filters.Operation;
using WebApi.Contexts.Import.Services;

namespace WebApi.Contexts.Import;

public static class AddContextExtension
{
    public static void AddImportContext(this IServiceCollection services)
    {
        services.AddScoped<ImportKeyService>();
        services.AddScoped<GradeImportService>();
        services.AddScoped<RequireImportKeyFilter>();
    }

    public static void AddImportContextOperationFilters(this SwaggerGenOptions options)
    {
        options.OperationFilter<RequireImportKeyOperationFilter>();
    }
}