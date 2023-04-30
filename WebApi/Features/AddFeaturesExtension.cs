using WebApi.Features.Auth.Options;
using WebApi.Features.Status.Options;

namespace WebApi.Features;

public static class AddFeaturesExtension
{
    public static void AddFeatures(this IServiceCollection services, IConfiguration configuration)
    {
        // Status
        services.Configure<StatusOptions>(configuration.GetSection("Status"));

        // Auth
        services.Configure<RefreshOptions>(configuration.GetSection("Refresh"));
    }
}