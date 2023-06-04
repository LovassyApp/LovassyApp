using Blueboard.Core.Auth.Jobs;
using Helpers.WebApi.Interfaces;
using Quartz;

namespace Blueboard.Core.Auth.StartupActions;

public class ScheduleAuthJobsAction : IStartupAction
{
    private readonly ILogger<ScheduleAuthJobsAction> _logger;
    private readonly ISchedulerFactory _schedulerFactory;

    public ScheduleAuthJobsAction(ISchedulerFactory schedulerFactory,
        ILogger<ScheduleAuthJobsAction> logger)
    {
        _schedulerFactory = schedulerFactory;
        _logger = logger;
    }

    public async Task Execute()
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var deleteOldTokensJob = JobBuilder.Create<DeleteOldTokensJob>()
            .WithIdentity("deleteOldTokens", "scheduledAuthJobs").Build();

        var deleteOldTokensTrigger = TriggerBuilder.Create().WithIdentity("deleteOldTokensTrigger", "scheduledAuthJobs")
            .StartNow()
            .WithSchedule(CronScheduleBuilder.CronSchedule("0 0 0 ? * MON"))
            .Build();

        await scheduler.ScheduleJob(deleteOldTokensJob, deleteOldTokensTrigger);

        _logger.LogInformation($"Added {nameof(DeleteOldTokensJob)} to recurring jobs");
    }
}