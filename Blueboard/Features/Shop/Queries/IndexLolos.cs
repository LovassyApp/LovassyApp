using Blueboard.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Shop.Queries;

public static class IndexLolos
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public bool IsSpent { get; set; }

        public string LoloType { get; set; }
        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, SieveProcessor sieveProcessor)
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var lolos = context.Lolos.AsNoTracking();

            var filteredLolos = await sieveProcessor.Apply(request.SieveModel, lolos).ToListAsync(cancellationToken);

            return filteredLolos.Adapt<IEnumerable<Response>>();
        }
    }
}