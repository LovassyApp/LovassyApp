using Blueboard.Features.Feed.Services;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Feed.Queries;

public static class IndexFeedItems
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Link { get; set; }
    }

    internal sealed class Handler
        (FeedService feedService, SieveProcessor sieveProcessor) : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var feedItems = feedService.FeedItems.AsQueryable();

            var filteredFeedItems = sieveProcessor.Apply(request.SieveModel, feedItems).ToList();

            return filteredFeedItems.Adapt<IEnumerable<Response>>();
        }
    }
}