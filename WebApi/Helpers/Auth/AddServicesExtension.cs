using WebApi.Helpers.Auth.Services;
using SessionOptions = WebApi.Helpers.Auth.Services.Options.SessionOptions;

namespace WebApi.Helpers.Auth;

public static class AddServicesExtension
{
    public static void AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SessionOptions>(configuration.GetSection("Session"));
        services.AddScoped<SessionManager>();
    }
}