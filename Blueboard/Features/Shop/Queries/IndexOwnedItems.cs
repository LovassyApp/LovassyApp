using Blueboard.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Shop.Queries;

public static class IndexOwnedItems
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
        public string? Search { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public ResponseProduct Product { get; set; }

        public DateTime? UsedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseProduct
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool QRCodeActivated { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly ApplicationDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public Handler(ApplicationDbContext context, SieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var ownedItems = _context.OwnedItems.Include(i => i.Product).AsNoTracking();

            var filteredOwnedItems = _sieveProcessor.Apply(request.SieveModel, ownedItems);

            if (!string.IsNullOrEmpty(request.Search))
            {
                var searchQuery = string.Join(" & ", request.Search.Split(" ").Select(w => w + ":*"));
                filteredOwnedItems = filteredOwnedItems.Where(i =>
                    i.Product.SearchVector.Matches(EF.Functions.ToTsQuery("hungarian", searchQuery)));
            }

            return (await filteredOwnedItems.ToListAsync(cancellationToken)).Adapt<IEnumerable<Response>>();
        }
    }
}