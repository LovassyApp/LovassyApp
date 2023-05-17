using Helpers.Cryptography.Services;
using Helpers.Cryptography.Services.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Helpers.Cryptography;

public static class AddCryptographyHelpersExtension
{
    public static void AddCryptographyHelpers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataProtection();

        services.Configure<EncryptionOptions>(configuration.GetSection("Cryptography"));
        services.AddSingleton<EncryptionService>();

        services.Configure<HashOptions>(configuration.GetSection("Cryptography"));
        services.AddSingleton<HashService>();
    }
}