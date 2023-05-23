using Helpers.Framework.Exceptions;
using MediatR;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Commands;

public static class DeleteUserGroup
{
    public class Command : IRequest
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var userGroup = await _context.UserGroups.FindAsync(request.Id);

            if (userGroup == null)
                throw new NotFoundException(nameof(UserGroup), request.Id);

            _context.UserGroups.Remove(userGroup);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}