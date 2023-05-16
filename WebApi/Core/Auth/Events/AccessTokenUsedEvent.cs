using MediatR;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Events;

public class AccessTokenUsedEvent : INotification
{
    public PersonalAccessToken AccessToken { get; set; }
}