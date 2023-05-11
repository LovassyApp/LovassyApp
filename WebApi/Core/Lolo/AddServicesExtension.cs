using WebApi.Core.Lolo.Services;
using WebApi.Core.Lolo.Services.Options;

namespace WebApi.Core.Lolo;

public static class AddServicesExtension
{
    public static void AddLoloServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LoloOptions>(configuration.GetSection("Lolo"));
        services.AddScoped<LoloManager>();
    }
}