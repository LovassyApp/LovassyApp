using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Shop.Queries;

public static class IndexOwnLoloRequests
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

        public DateTime? AcceptedAt { get; set; }
        public DateTime? DeniedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly ApplicationDbContext _context;
        private readonly SieveProcessor _sieveProcessor;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, SieveProcessor sieveProcessor, UserAccessor userAccessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
            _userAccessor = userAccessor;
        }

        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var loloRequests = _context.LoloRequests.Where(r => r.UserId == _userAccessor.User.Id).AsNoTracking();

            var filteredLoloRequests =
                await _sieveProcessor.Apply(request.SieveModel, loloRequests).ToListAsync(cancellationToken);

            return filteredLoloRequests.Adapt<IEnumerable<Response>>();
        }
    }
}