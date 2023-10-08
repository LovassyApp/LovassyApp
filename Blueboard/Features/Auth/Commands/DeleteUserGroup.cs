using Blueboard.Core.Auth;
using Blueboard.Features.Auth.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var userGroup = await _context.UserGroups.Include(g => g.UploadableImageVotings)
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (userGroup == null)
                throw new NotFoundException(nameof(UserGroup), request.Id);

            if (userGroup.Id == AuthConstants.DefaultUserGroupID)
                throw new BadRequestException("Nem törölheted az alapértelmezett csoportot.");

            if (userGroup.UploadableImageVotings.Count > 0)
                throw new BadRequestException("Nem törölheted a csoportot, mert szavazás van hozzárendelve.");

            _context.UserGroups.Remove(userGroup);
            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new UserGroupsUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}