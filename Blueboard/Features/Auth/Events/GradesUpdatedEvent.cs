using MediatR;

namespace Blueboard.Features.Auth.Events;

public class GradesUpdatedEvent : INotification
{
    public Guid UserId { get; set; }
}