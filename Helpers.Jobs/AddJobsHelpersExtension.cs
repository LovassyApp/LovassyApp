using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Helpers.Jobs;

public static class AddJobsHelpersExtension
{
    public static void AddJobsHelpers(this IServiceCollection services)
    {
        // Scheduler
        services.AddQuartz(q => { q.UseMicrosoftDependencyInjectionJobFactory(); });
        services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });
    }
}