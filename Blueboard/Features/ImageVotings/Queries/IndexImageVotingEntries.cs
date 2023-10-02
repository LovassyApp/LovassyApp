using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.ImageVotings.Queries;

public static class IndexImageVotingEntries
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; } = null!;
    }

    public class Response
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        [AdaptIgnore] public Guid? UserId { get; set; }
        [AdaptIgnore] public ResponseUser? User { get; set; }

        public int ImageVotingId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseUser
    {
        public string Name { get; set; }
        public string? RealName { get; set; }
        public string? Class { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly ApplicationDbContext _context;
        private readonly PermissionManager _permissionManager;
        private readonly SieveProcessor _sieveProcessor;

        public Handler(ApplicationDbContext context, PermissionManager permissionManager, SieveProcessor sieveProcessor)
        {
            _context = context;
            _permissionManager = permissionManager;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entries = _context.ImageVotingEntries.Include(e => e.ImageVoting).Include(e => e.User)
                .AsNoTracking();

            if (!_permissionManager.CheckPermission(typeof(ImageVotingsPermissions.IndexImageVotingEntries)))
                entries = entries.Where(e => e.ImageVoting.Active);

            var response = (await _sieveProcessor.Apply(request.SieveModel, entries).ToListAsync(cancellationToken))
                .Select(entry => CreateResponse(entry, entry.ImageVoting.ShowUploaderInfo));

            return response;
        }

        private Response CreateResponse(ImageVotingEntry entry, bool includeUser = true)
        {
            var response = entry.Adapt<Response>();

            if (includeUser)
            {
                response.User = entry.User.Adapt<ResponseUser>();
                response.UserId = entry.UserId;
            }

            return response;
        }
    }
}