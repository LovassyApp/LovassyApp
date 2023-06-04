using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;

namespace Blueboard.Features.Users.Events;

public class UserCreatedEvent : INotification
{
    public User User { get; set; }
    public string VerifyUrl { get; set; }
    public string VerifyTokenQueryKey { get; set; }
}