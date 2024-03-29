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
    public enum ResponseIncrementType
    {
        None,
        Incremented,
        SuperIncremented,
        Decremented
    }

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

        public bool? CanChoose { get; set; }
        public bool? Chosen { get; set; }

        public string? IncrementType { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseUser
    {
        public string Name { get; set; }
        public string? RealName { get; set; }
        public string? Class { get; set; }
    }

    internal sealed class Handler(
        ApplicationDbContext context,
        PermissionManager permissionManager,
        SieveProcessor sieveProcessor,
        UserAccessor userAccessor)
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entries = context.ImageVotingEntries
                .Include(e => e.Increments.Where(i => i.UserId == userAccessor.User.Id)).Include(e => e.ImageVoting)
                .ThenInclude(v => v.Choices.Where(c => c.UserId == userAccessor.User.Id)).Include(e => e.User)
                .AsNoTracking(); //I verified that there is no cartesian explosion here

            if (!permissionManager.CheckPermission(typeof(ImageVotingsPermissions.IndexImageVotingEntries)))
                entries = entries.Where(e => e.ImageVoting.Active);

            var response = (await sieveProcessor.Apply(request.SieveModel, entries).ToListAsync(cancellationToken))
                .Select(entry => CreateResponse(entry));

            return response;
        }

        private Response CreateResponse(ImageVotingEntry entry)
        {
            var response = entry.Adapt<Response>();

            if (entry.ImageVoting.ShowUploaderInfo)
            {
                response.User = entry.User.Adapt<ResponseUser>();
                response.UserId = entry.UserId;
            }

            if (entry.ImageVoting.Type == ImageVotingType.SingleChoice && !entry.ImageVoting.Aspects.Any())
            {
                response.CanChoose = true;
                response.Chosen = entry.ImageVoting.Choices.Any(c => c.ImageVotingEntryId == entry.Id);
            }

            if (entry.ImageVoting.Type == ImageVotingType.Increment && !entry.ImageVoting.Aspects.Any())
            {
                var increment = entry.Increments.FirstOrDefault(i => i.UserId == userAccessor.User.Id);
                response.IncrementType = (increment != null
                    ? increment.Increment == entry.ImageVoting.SuperIncrementValue
                        ? ResponseIncrementType.SuperIncremented
                        : increment.Increment < 0
                            ? ResponseIncrementType.Decremented
                            : ResponseIncrementType.Incremented
                    : ResponseIncrementType.None).Adapt<string>();
            }

            if (entry.UserId == userAccessor.User.Id)
                response.CanChoose = false;

            return response;
        }
    }
}