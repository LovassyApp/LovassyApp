using Blueboard.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Users.Queries;

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

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseUserGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, SieveProcessor sieveProcessor)
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var users = context.Users
                .Include(u => u.UserGroups)
                .AsNoTracking();

            var filteredUsers = await sieveProcessor.Apply(request.SieveModel, users).ToListAsync(cancellationToken);

            return filteredUsers.Adapt<IEnumerable<Response>>();
        }
    }
}