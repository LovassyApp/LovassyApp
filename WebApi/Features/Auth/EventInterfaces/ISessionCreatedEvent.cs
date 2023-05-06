using MediatR;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.EventInterfaces;

public interface ISessionCreatedEvent : INotification
{
    public User User { get; set; }
}