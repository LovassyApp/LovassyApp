using Blueboard.Core.Auth.Jobs;
using Helpers.WebApi.Interfaces;
using Quartz;

namespace Blueboard.Core.Auth.StartupActions;

public class ScheduleAuthJobsAction(ISchedulerFactory schedulerFactory,
        ILogger<ScheduleAuthJobsAction> logger)
    : IStartupAction
{
    public async Task Execute()
    {
        var scheduler = await schedulerFactory.GetScheduler();

        var deleteOldTokensJob = JobBuilder.Create<DeleteOldTokensJob>()
            .WithIdentity("deleteOldTokens", "scheduledAuthJobs").Build();

        var deleteOldTokensTrigger = TriggerBuilder.Create().WithIdentity("deleteOldTokensTrigger", "scheduledAuthJobs")
            .StartNow()
            .WithSchedule(CronScheduleBuilder.CronSchedule("0 0 0 ? * MON"))
            .Build();

        await scheduler.ScheduleJob(deleteOldTokensJob, deleteOldTokensTrigger);

        logger.LogInformation($"Added {nameof(DeleteOldTokensJob)} to recurring jobs");
    }
}