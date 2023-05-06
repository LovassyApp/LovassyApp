using Sieve.Services;

namespace WebApi.Core.Filtering;

public static class AddServicesExtension
{
    public static void AddFilteringServices(this IServiceCollection services)
    {
        services.AddScoped<SieveProcessor, ApplicationSieveProcessor>();
    }
}