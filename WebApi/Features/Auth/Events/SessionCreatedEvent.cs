using MediatR;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Events;

public class SessionCreatedEvent : INotification
{
    public User User { get; set; }
    public string MasterKey { get; set; }
}