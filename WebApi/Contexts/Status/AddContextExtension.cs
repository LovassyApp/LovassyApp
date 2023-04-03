using WebApi.Contexts.Status.Services;
using WebApi.Contexts.Status.Services.Options;

namespace WebApi.Contexts.Status;

public static class AddContextExtension
{
    public static void AddStatusContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StatusOptions>(configuration.GetSection("Status"));
        services.AddScoped<IStatusService, StatusService>();
    }
}