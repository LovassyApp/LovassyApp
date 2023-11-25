using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.ImageVotings.Queries;

public static class ViewImageVotingChoice
{
    public class Query : IRequest<Response>
    {
        public int Id { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string? AspectKey { get; set; }

        public ResponseImageVoting ImageVoting { get; set; }
        public ResponseImageVotingEntry ImageVotingEntry { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseImageVoting
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Type { get; set; }

        public List<ResponseImageVotingAspect> Aspects { get; set; }

        public bool Active { get; set; }
        public bool ShowUploaderInfo { get; set; }

        public bool SuperIncrementAllowed { get; set; }
        public int SuperIncrementValue { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseImageVotingAspect
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class ResponseImageVotingEntry
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string ImageUrl { get; set; }

        [AdaptIgnore] public Guid? UserId { get; set; }
        [AdaptIgnore] public ResponseImageVotingEntryUser? User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseImageVotingEntryUser
    {
        public string Name { get; set; }
        public string? RealName { get; set; }
        public string? Class { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, PermissionManager permissionManager)
        : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var choice = await context.ImageVotingChoices
                .Include(c => c.ImageVoting)
                .Include(c => c.ImageVotingEntry)
                .ThenInclude(e => e.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (choice == null)
                throw new NotFoundException(nameof(ImageVotingChoice), request.Id);

            if (!choice.ImageVoting.Active &&
                !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.ViewImageVotingChoice)))
                throw new NotFoundException(nameof(ImageVotingChoice), request.Id);

            var response = choice.Adapt<Response>();

            if (choice.ImageVoting.ShowUploaderInfo)
            {
                response.ImageVotingEntry.User = choice.ImageVotingEntry.User.Adapt<ResponseImageVotingEntryUser>();
                response.UserId = choice.ImageVotingEntry.User.Id;
            }

            return response;
        }
    }
}