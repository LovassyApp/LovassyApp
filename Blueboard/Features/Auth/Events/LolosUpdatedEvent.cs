using MediatR;

namespace Blueboard.Features.Auth.Events;

public class LolosUpdatedEvent : INotification
{
    public Guid UserId { get; set; }
}