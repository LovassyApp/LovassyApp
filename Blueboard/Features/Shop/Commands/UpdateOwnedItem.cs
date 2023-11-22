using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;

namespace Blueboard.Features.Shop.Commands;

public static class UpdateOwnedItem
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public DateTime? UsedAt { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, IPublisher publisher) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var ownedItem = await context.OwnedItems.FindAsync(request.Id);

            if (ownedItem == null) throw new NotFoundException(nameof(OwnedItem), request.Id);

            request.Body.Adapt(ownedItem);

            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new OwnedItemUpdatedEvent
            {
                UserId = ownedItem.UserId
            }, cancellationToken);

            return Unit.Value;
        }
    }
}