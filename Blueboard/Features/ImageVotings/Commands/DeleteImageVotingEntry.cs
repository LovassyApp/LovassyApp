using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.ImageVotings.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.ImageVotings.Commands;

public static class DeleteImageVotingEntry
{
    public class Command : IRequest
    {
        public int Id { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, UserAccessor userAccessor, IPublisher publisher,
            PermissionManager permissionManager)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var imageVotingEntry = await context.ImageVotingEntries.FindAsync(request.Id);

            if (imageVotingEntry == null)
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            if (imageVotingEntry.UserId != userAccessor.User.Id &&
                !permissionManager.CheckPermission(typeof(ImageVotingsPermissions.DeleteImageVotingEntry)))
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            context.ImageVotingEntries.Remove(imageVotingEntry);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new ImageVotingEntriesUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}