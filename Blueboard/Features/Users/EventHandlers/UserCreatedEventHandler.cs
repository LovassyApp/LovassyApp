using Blueboard.Core.Auth;
using Blueboard.Features.Users.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Users.EventHandlers;

public class UserCreatedEventHandler(ApplicationDbContext context) : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var userGroup = new UserGroup
        {
            Id = AuthConstants.DefaultUserGroupID
        };
        context.Entry(userGroup).State = EntityState.Unchanged;

        notification.User.UserGroups = new List<UserGroup> { userGroup };

        await context.SaveChangesAsync(cancellationToken);
    }
}