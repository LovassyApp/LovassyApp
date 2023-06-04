using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;

namespace Blueboard.Core.Auth.Events;

/// <summary>
///     The event fired when a <see cref="PersonalAccessToken" /> is used to authenticate a user.
/// </summary>
public class AccessTokenUsedEvent : INotification
{
    public PersonalAccessToken AccessToken { get; set; }
}