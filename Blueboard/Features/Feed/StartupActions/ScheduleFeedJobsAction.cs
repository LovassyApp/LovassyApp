using Blueboard.Features.Feed.Jobs;
using Helpers.WebApi.Interfaces;
using Quartz;
using Shimmer.Services;

namespace Blueboard.Features.Feed.StartupActions;

public class ScheduleFeedJobsAction(ILogger<ScheduleFeedJobsAction> logger, IShimmerJobFactory jobFactory)
    : IStartupAction
{
    public async Task Execute()
    {
        var updateFeedJob = await jobFactory.CreateAsync<UpdateFeedJob>();

        updateFeedJob.Schedule(SimpleScheduleBuilder.RepeatHourlyForever(12));

        await updateFeedJob.FireAsync();

        logger.LogInformation($"Added {nameof(UpdateFeedJob)} to recurring jobs");
    }
}