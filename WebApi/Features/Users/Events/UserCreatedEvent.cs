using MediatR;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Users.Events;

public class UserCreatedEvent : INotification
{
    public User User { get; set; }
    public string VerifyUrl { get; set; }
    public string VerifyTokenQueryKey { get; set; }
}