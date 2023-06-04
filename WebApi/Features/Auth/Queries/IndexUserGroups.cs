using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Auth.Queries;

public static class IndexUserGroups
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Permissions { get; set; }

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
            var userGroups = _context.UserGroups.AsNoTracking();
            var filteredUserGroups =
                await _sieveProcessor.Apply(request.SieveModel, userGroups).ToListAsync(cancellationToken);
            return filteredUserGroups.Adapt<IEnumerable<Response>>();
        }
    }
}