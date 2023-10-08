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

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly PermissionManager _permissionManager;
        private readonly IPublisher _publisher;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, UserAccessor userAccessor, IPublisher publisher,
            PermissionManager permissionManager)
        {
            _context = context;
            _userAccessor = userAccessor;
            _publisher = publisher;
            _permissionManager = permissionManager;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var imageVotingEntry = await _context.ImageVotingEntries.FindAsync(request.Id);

            if (imageVotingEntry == null)
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            if (imageVotingEntry.UserId != _userAccessor.User.Id &&
                !_permissionManager.CheckPermission(typeof(ImageVotingsPermissions.DeleteImageVotingEntry)))
                throw new NotFoundException(nameof(ImageVotingEntry), request.Id);

            _context.ImageVotingEntries.Remove(imageVotingEntry);
            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new ImageVotingEntriesUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}