using Hangfire;
using Hangfire.PostgreSql;

namespace WebApi.Core.Scheduler;

public static class AddServicesExtension
{
    public static void AddSchedulerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(c => c.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(configuration.GetConnectionString("HangfireConnection")));
        services.AddHangfireServer();
    }
}