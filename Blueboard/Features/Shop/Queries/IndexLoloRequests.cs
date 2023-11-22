using Blueboard.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Shop.Queries;

public static class IndexLoloRequests
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }

        public Guid UserId { get; set; }

        public DateTime? AcceptedAt { get; set; }
        public DateTime? DeniedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, SieveProcessor sieveProcessor)
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var loloRequests = context.LoloRequests.AsNoTracking();

            var filteredLoloRequests =
                await sieveProcessor.Apply(request.SieveModel, loloRequests).ToListAsync(cancellationToken);

            return filteredLoloRequests.Adapt<IEnumerable<Response>>();
        }
    }
}