using MediatR;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Events;

/// <summary>
///     The event fired when a <see cref="PersonalAccessToken" /> is used to authenticate a user.
/// </summary>
public class AccessTokenUsedEvent : INotification
{
    public PersonalAccessToken AccessToken { get; set; }
}