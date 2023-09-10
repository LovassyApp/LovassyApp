using Blueboard.Features.Feed.Jobs;
using Helpers.WebApi.Interfaces;
using Quartz;

namespace Blueboard.Features.Feed.StartupActions;

public class ScheduleFeedJobsAction : IStartupAction
{
    private readonly ILogger<ScheduleFeedJobsAction> _logger;
    private readonly ISchedulerFactory _schedulerFactory;

    public ScheduleFeedJobsAction(ILogger<ScheduleFeedJobsAction> logger, ISchedulerFactory schedulerFactory)
    {
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    public async Task Execute()
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var updateFeedJob = JobBuilder.Create<UpdateFeedJob>()
            .WithIdentity("updateFeed", "scheduledFeedJobs").Build();

        var updateFeedTrigger = TriggerBuilder.Create().WithIdentity("updateFeedTrigger", "scheduledFeedJobs")
            .StartNow()
            .WithSchedule(SimpleScheduleBuilder.RepeatHourlyForever(12))
            .Build();

        await scheduler.ScheduleJob(updateFeedJob, updateFeedTrigger);

        _logger.LogInformation($"Added {nameof(UpdateFeedJob)} to recurring jobs");
    }
}