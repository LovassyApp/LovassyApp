using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Users.Queries;

public static class IndexUsers
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public string? RealName { get; set; }
        public string? Class { get; set; }

        public List<ResponseUserGroup> UserGroups { get; set; }
    }

    public class ResponseUserGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
            var users = _context.Users
                .Include(u => u.UserGroups)
                .AsNoTracking();

            var filteredUsers = await _sieveProcessor.Apply(request.SieveModel, users).ToListAsync(cancellationToken);

            return filteredUsers.Adapt<IEnumerable<Response>>();
        }
    }
}