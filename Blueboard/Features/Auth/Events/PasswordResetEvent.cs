using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;

namespace Blueboard.Features.Auth.Events;

public class PasswordResetEvent : INotification
{
    public User User { get; set; }
}