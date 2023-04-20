using WebApi.Helpers.Auth.Services;

namespace WebApi.Helpers.Auth;

public static class AddServicesExtension
{
    public static void AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<SessionManager>();
    }
}