using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.Shop.Commands;

public static class DeleteLoloRequest
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

        public Handler(ApplicationDbContext context, UserAccessor userAccessor, PermissionManager permissionManager)
        {
            _context = context;
            _userAccessor = userAccessor;
            _permissionManager = permissionManager;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var loloRequest = await _context.LoloRequests.FindAsync(request.Id);

            if (loloRequest == null)
                throw new NotFoundException(nameof(LoloRequest), request.Id);

            if (loloRequest.UserId != _userAccessor.User!.Id &&
                !_permissionManager.CheckPermission(typeof(ShopPermissions.DeleteLoloRequest)))
                throw new ForbiddenException();

            if (loloRequest.AcceptedAt != null || loloRequest.DeniedAt != null)
                throw new BadRequestException(
                    "Ez a kérvény már nem törölhető. (el lett fogadva vagy el lett utasítva)");

            _context.LoloRequests.Remove(loloRequest);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}