using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.ImageVotings.Queries;

public static class ViewImageVotingEntry
{
    public class Query : IRequest<Response>
    {
        public int Id { get; set; }
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

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseUser
    {
        public string Name { get; set; }
        public string? RealName { get; set; }
        public string? Class { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, PermissionManager permissionManager,
            UserAccessor userAccessor)
        : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var entry = await context.ImageVotingEntries
                .Include(e => e.User)
                .Include(e => e.ImageVoting)
                .ThenInclude(i => i.Choices)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (entry == null)
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            if (!entry.ImageVoting.Active &&
                !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.ViewImageVotingEntry)))
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            var response = entry.Adapt<Response>();

            if (entry.ImageVoting.ShowUploaderInfo)
            {
                response.User = entry.User.Adapt<ResponseUser>();
                response.UserId = entry.User.Id;
            }

            if (entry.ImageVoting.Type == ImageVotingType.SingleChoice && !entry.ImageVoting.Aspects.Any())
            {
                response.CanChoose = !entry.ImageVoting.Choices.Any();

                response.Chosen = entry.ImageVoting.Choices.Any(c => c.ImageVotingEntryId == entry.Id);
            }

            if (entry.UserId == userAccessor.User.Id)
                response.CanChoose = false;

            return response;
        }
    }
}