using Blueboard.Features.Feed.Events;
using Blueboard.Features.Feed.Services;
using MediatR;
using Quartz;

namespace Blueboard.Features.Feed.Jobs;

public class UpdateFeedJob : IJob
{
    private readonly FeedService _feedService;
    private readonly ILogger<UpdateFeedJob> _logger;
    private readonly IPublisher _publisher;

    public UpdateFeedJob(FeedService feedService, ILogger<UpdateFeedJob> logger, IPublisher publisher)
    {
        _feedService = feedService;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Updating feed");
        try
        {
            await _feedService.UpdateFeed();
            await _publisher.Publish(new FeedItemsUpdatedEvent());
            _logger.LogInformation("Feed updated");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating feed");
        }
    }
}