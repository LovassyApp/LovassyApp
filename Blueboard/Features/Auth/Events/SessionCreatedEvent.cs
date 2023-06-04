using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;

namespace Blueboard.Features.Auth.Events;

public class SessionCreatedEvent : INotification
{
    public User User { get; set; }
    public string MasterKey { get; set; }
}