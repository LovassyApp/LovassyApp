using WebApi.Helpers.Cryptography.Services;
using WebApi.Helpers.Cryptography.Services.Options;

namespace WebApi.Helpers.Cryptography;

public static class AddServicesExtension
{
    public static void AddCryptographyServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataProtection();
        
        services.Configure<EncryptionOptions>(configuration.GetSection("Cryptography"));
        services.AddSingleton<IEncryptionService, EncryptionService>();
    }
}