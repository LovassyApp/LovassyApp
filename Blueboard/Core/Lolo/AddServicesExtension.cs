using Blueboard.Core.Lolo.Services;
using Blueboard.Core.Lolo.Services.Options;

namespace Blueboard.Core.Lolo;

public static class AddServicesExtension
{
    public static void AddLoloServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LoloOptions>(configuration.GetSection("Lolo"));
        services.AddScoped<LoloManager>();
    }
}