using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
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

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly PermissionManager _permissionManager;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, PermissionManager permissionManager, UserAccessor userAccessor)
        {
            _context = context;
            _permissionManager = permissionManager;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var ownedItem = await _context.OwnedItems.FindAsync(request.Id);

            if (ownedItem == null)
                throw new NotFoundException(nameof(OwnedItem), request.Id);

            if (ownedItem.UserId != _userAccessor.User.Id &&
                !_permissionManager.CheckPermission(typeof(ShopPermissions.DeleteOwnedItem)))
                throw new NotFoundException(nameof(OwnedItem), request.Id);

            _context.OwnedItems.Remove(ownedItem);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}