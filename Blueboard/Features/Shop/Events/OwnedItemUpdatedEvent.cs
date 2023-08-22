using MediatR;

namespace Blueboard.Features.Shop.Events;

public class OwnedItemUpdatedEvent : INotification
{
    public Guid UserId { get; set; }
}