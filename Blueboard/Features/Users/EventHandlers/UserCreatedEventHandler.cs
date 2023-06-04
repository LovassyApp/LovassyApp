using Blueboard.Core.Auth;
using Blueboard.Features.Users.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Users.EventHandlers;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ApplicationDbContext _context;

    public UserCreatedEventHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var userGroup = new UserGroup
        {
            Id = AuthConstants.DefaultUserGroupID
        };
        _context.Entry(userGroup).State = EntityState.Unchanged;

        notification.User.UserGroups = new List<UserGroup> { userGroup };

        await _context.SaveChangesAsync(cancellationToken);
    }
}