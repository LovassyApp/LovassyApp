using MediatR;

namespace Blueboard.Features.Shop.Events;

public class LoloRequestUpdatedEvent : INotification
{
    public Guid UserId { get; set; }
}