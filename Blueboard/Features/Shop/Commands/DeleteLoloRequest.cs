using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.Shop.Events;
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

    internal sealed class Handler(ApplicationDbContext context, UserAccessor userAccessor,
            PermissionManager permissionManager, IPublisher publisher)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var loloRequest = await context.LoloRequests.FindAsync(request.Id);

            if (loloRequest == null)
                throw new NotFoundException(nameof(LoloRequest), request.Id);

            if (loloRequest.UserId != userAccessor.User.Id &&
                !permissionManager.CheckPermission(typeof(ShopPermissions.DeleteLoloRequest)))
                throw new ForbiddenException();

            if (loloRequest.AcceptedAt != null || loloRequest.DeniedAt != null)
                throw new BadRequestException(
                    "Ez a kérvény már nem törölhető. (el lett fogadva vagy el lett utasítva)");

            context.LoloRequests.Remove(loloRequest);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new LoloRequestUpdatedEvent
            {
                UserId = loloRequest.UserId
            }, cancellationToken);

            return Unit.Value;
        }
    }
}