using Helpers.Filtering.Services;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;

namespace Helpers.Filtering;

public static class AddFilteringHelpersExtension
{
    public static void AddFilteringHelpers(this IServiceCollection services)
    {
        services.AddScoped<SieveProcessor, ApplicationSieveProcessor>();
    }
}