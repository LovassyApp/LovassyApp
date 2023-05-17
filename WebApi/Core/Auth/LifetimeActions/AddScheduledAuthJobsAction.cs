using Helpers.Framework.Interfaces;
using Quartz;
using WebApi.Core.Auth.Jobs;

namespace WebApi.Core.Auth.LifetimeActions;

public class AddScheduledAuthJobsAction : ILifetimeAction
{
    private readonly ILogger<AddScheduledAuthJobsAction> _logger;
    private readonly ISchedulerFactory _schedulerFactory;

    public AddScheduledAuthJobsAction(ISchedulerFactory schedulerFactory,
        ILogger<AddScheduledAuthJobsAction> logger)
    {
        _schedulerFactory = schedulerFactory;
        _logger = logger;
    }

    public async Task OnStartAsync(CancellationToken cancellationToken)
    {
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

        var deleteOldTokensJob = JobBuilder.Create<DeleteOldTokensJob>()
            .WithIdentity("deleteOldTokens", "scheduledAuthJobs").Build();

        var deleteOldTokensTrigger = TriggerBuilder.Create().WithIdentity("deleteOldTokensTrigger", "scheduledAuthJobs")
            .StartNow()
            .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromDays(1)).RepeatForever())
            .Build();

        await scheduler.ScheduleJob(deleteOldTokensJob, deleteOldTokensTrigger, cancellationToken);

        _logger.LogInformation($"Added {nameof(DeleteOldTokensJob)} to recurring jobs");
    }

    public async Task OnStopAsync(CancellationToken cancellationToken)
    {
    }
}