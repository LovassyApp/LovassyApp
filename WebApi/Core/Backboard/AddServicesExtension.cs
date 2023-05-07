using WebApi.Core.Backboard.Services;
using WebApi.Core.Backboard.Services.Options;

namespace WebApi.Core.Backboard;

public static class AddServicesExtension
{
    /// <summary>
    ///     Adds all Backboard related services that are likely to be used either directly or indirectly in some features.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app configuration.</param>
    public static void AddBackboardServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<BackboardAdapter>();
        services.Configure<BackboardOptions>(configuration.GetSection("Backboard"));
    }
}