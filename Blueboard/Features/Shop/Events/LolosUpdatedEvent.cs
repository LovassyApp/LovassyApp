using MediatR;

namespace Blueboard.Features.Shop.Events;

public class LolosUpdatedEvent : INotification
{
    public Guid UserId { get; set; }
}