using WebApi.Contexts.Import.Services;

namespace WebApi.Contexts.Import;

public static class AddContextExtension
{
    public static void AddImportContext(this IServiceCollection services)
    {
        services.AddScoped<ImportKeyService>();
        services.AddScoped<GradeImportService>();
    }
}