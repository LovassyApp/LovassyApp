using Helpers.Framework.Interfaces;
using Quartz;
using WebApi.Core.Auth.Jobs;

namespace WebApi.Core.Auth.StartupActions;

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
            .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromDays(1)).RepeatForever())
            .Build();

        await scheduler.ScheduleJob(deleteOldTokensJob, deleteOldTokensTrigger);

        _logger.LogInformation($"Added {nameof(DeleteOldTokensJob)} to recurring jobs");
    }
}