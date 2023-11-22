using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.Users.Commands;

public static class DeleteUser
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, UserAccessor userAccessor) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(request.Id, cancellationToken);

            if (user == null)
                throw new NotFoundException(nameof(User), request.Id);

            if (user.Id == userAccessor.User.Id)
                throw new BadRequestException("Nem törölheted saját magad.");

            context.Users.Remove(user);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}