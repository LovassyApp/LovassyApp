using WebApi.Core.Cryptography.Services;
using WebApi.Core.Cryptography.Services.Options;

namespace WebApi.Core.Cryptography;

public static class AddServicesExtension
{
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