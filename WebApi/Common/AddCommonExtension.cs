using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using WebApi.Common.Behaviours;

namespace WebApi.Common;

public static class AddCommonExtension
{
    public static void AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        // MediatR
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        // Scheduler
        services.AddHangfire(c => c.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(configuration.GetConnectionString("HangfireConnection")));
        services.AddHangfireServer();
    }
}