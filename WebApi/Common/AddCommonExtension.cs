using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using WebApi.Common.Behaviours;
using WebApi.Common.Services;

namespace WebApi.Common;

public static class AddCommonExtension
{
    public static void AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        // MediatR
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        // Services
        services.AddScoped<DomainEventService>();

        // Scheduler
        services.AddHangfire(c => c.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(configuration.GetConnectionString("HangfireConnection")));
        services.AddHangfireServer();
    }
}