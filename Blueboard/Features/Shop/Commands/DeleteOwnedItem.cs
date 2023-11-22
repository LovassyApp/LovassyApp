using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.Shop.Commands;

public static class DeleteOwnedItem
{
    public class Command : IRequest
    {
        public int Id { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, PermissionManager permissionManager,
            UserAccessor userAccessor, IPublisher publisher)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var ownedItem = await context.OwnedItems.FindAsync(request.Id);

            if (ownedItem == null)
                throw new NotFoundException(nameof(OwnedItem), request.Id);

            if (ownedItem.UserId != userAccessor.User.Id &&
                !permissionManager.CheckPermission(typeof(ShopPermissions.DeleteOwnedItem)))
                throw new NotFoundException(nameof(OwnedItem), request.Id);

            context.OwnedItems.Remove(ownedItem);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new OwnedItemUpdatedEvent
            {
                UserId = ownedItem.UserId
            }, cancellationToken);

            return Unit.Value;
        }
    }
}