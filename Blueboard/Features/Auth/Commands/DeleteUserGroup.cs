using Blueboard.Core.Auth;
using Blueboard.Features.Auth.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.Auth.Commands;

public static class DeleteUserGroup
{
    public class Command : IRequest
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublisher _publisher;

        public Handler(ApplicationDbContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var userGroup = await _context.UserGroups.FindAsync(request.Id);

            if (userGroup == null)
                throw new NotFoundException(nameof(UserGroup), request.Id);

            if (userGroup.Id == AuthConstants.DefaultUserGroupID)
                throw new BadRequestException("Nem törölheted az alapértelmezett csoportot.");

            _context.UserGroups.Remove(userGroup);
            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new UserGroupsUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}