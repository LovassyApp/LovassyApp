using WebApi.Core.Cryptography.Services;
using WebApi.Core.Cryptography.Services.Options;

namespace WebApi.Core.Cryptography;

public static class AddServicesExtension
{
    /// <summary>
    ///     Adds all services related to cryptography that are likely to be used either directly or indirectly in some
    ///     features.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app configuration.</param>
    public static void AddCryptographyServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataProtection();

        services.Configure<EncryptionOptions>(configuration.GetSection("Cryptography"));
        services.AddSingleton<EncryptionService>();

        services.Configure<HashOptions>(configuration.GetSection("Cryptography"));
        services.AddSingleton<HashService>();

        services.AddSingleton<ResetService>();

        services.AddScoped<EncryptionManager>();
        services.AddScoped<HashManager>();
    }
}