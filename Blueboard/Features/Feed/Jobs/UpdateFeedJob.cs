using Blueboard.Features.Feed.Events;
using Blueboard.Features.Feed.Services;
using MediatR;
using Quartz;
using Shimmer.Core;

namespace Blueboard.Features.Feed.Jobs;

public class UpdateFeedJob(FeedService feedService, ILogger<UpdateFeedJob> logger, IPublisher publisher)
    : ShimmerJob
{
    protected override async Task Process(IJobExecutionContext context)
    {
        logger.LogInformation("Updating feed");
        try
        {
            await feedService.UpdateFeed();
            await publisher.Publish(new FeedItemsUpdatedEvent());
            logger.LogInformation("Feed updated");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating feed");
        }
    }
}