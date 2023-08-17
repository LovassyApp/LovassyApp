using MediatR;

namespace Blueboard.Features.School.Events;

public class GradesUpdatedEvent : INotification
{
    public Guid UserId { get; set; }
}