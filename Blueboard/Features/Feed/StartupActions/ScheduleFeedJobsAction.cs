using Blueboard.Features.Feed.Jobs;
using Helpers.WebApi.Interfaces;
using Quartz;

namespace Blueboard.Features.Feed.StartupActions;

public class ScheduleFeedJobsAction(ILogger<ScheduleFeedJobsAction> logger, ISchedulerFactory schedulerFactory)
    : IStartupAction
{
    public async Task Execute()
    {
        var scheduler = await schedulerFactory.GetScheduler();

        var updateFeedJob = JobBuilder.Create<UpdateFeedJob>()
            .WithIdentity("updateFeed", "scheduledFeedJobs").Build();

        var updateFeedTrigger = TriggerBuilder.Create().WithIdentity("updateFeedTrigger", "scheduledFeedJobs")
            .StartNow()
            .WithSchedule(SimpleScheduleBuilder.RepeatHourlyForever(12))
            .Build();

        await scheduler.ScheduleJob(updateFeedJob, updateFeedTrigger);

        logger.LogInformation($"Added {nameof(UpdateFeedJob)} to recurring jobs");
    }
}