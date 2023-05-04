using WebApi.Core.Backboard.Services;
using WebApi.Core.Backboard.Services.Options;

namespace WebApi.Core.Backboard;

public static class AddServicesExtension
{
    public static void AddBackboardServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<BackboardAdapter>();
        services.Configure<BackboardOptions>(configuration.GetSection("Backboard"));
    }
}