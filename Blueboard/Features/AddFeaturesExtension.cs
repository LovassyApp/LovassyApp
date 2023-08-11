using Blueboard.Features.Auth.Services;
using Blueboard.Features.Auth.Services.Options;
using Blueboard.Features.Realtime;
using Blueboard.Features.Status.Services.Options;
using Microsoft.AspNetCore.Http.Connections;

namespace Blueboard.Features;

public static class AddFeaturesExtension
{
    /// <summary>
    ///     Adds all services related to isolated features. These services are not to be used outside of their respective
    ///     feature!
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app configuration.</param>
    public static void AddFeatures(this IServiceCollection services, IConfiguration configuration)
    {
        // Status
        services.Configure<StatusOptions>(configuration.GetSection("Status"));

        // Auth
        services.Configure<RefreshOptions>(configuration.GetSection("Refresh"));
        services.Configure<VerifyEmailOptions>(configuration.GetSection("VerifyEmail"));
        services.Configure<PasswordResetOptions>(configuration.GetSection("PasswordReset"));
        services.AddSingleton<RefreshService>();
        services.AddSingleton<VerifyEmailService>();
        services.AddSingleton<PasswordResetService>();
    }

    public static void MapFeatureHubs(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapHub<NotificationsHub>("/Hubs/Notifications",
            o =>
            {
                o.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling |
                               HttpTransportType.ServerSentEvents;
            });
    }
}