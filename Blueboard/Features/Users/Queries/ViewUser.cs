using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Users.Queries;

public static class ViewUser
{
    public class Query : IRequest<Response>
    {
        public Guid Id { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public string? RealName { get; set; }
        public string? Class { get; set; }

        public DateTime? LastOnlineAt { get; set; }

        public List<ResponseUserGroup> UserGroups { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseUserGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Permissions { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(u => u.Id == request.Id).Include(u => u.UserGroups)
                .Include(u =>
                    u.PersonalAccessTokens.Where(t => t.LastUsedAt != null).OrderByDescending(t => t.LastUsedAt))
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new NotFoundException(nameof(User), request.Id);

            var response = user.Adapt<Response>();

            response.LastOnlineAt = user.PersonalAccessTokens.FirstOrDefault()?.LastUsedAt;

            return response;
        }
    }
}