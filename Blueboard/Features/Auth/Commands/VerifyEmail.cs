using Blueboard.Features.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.Auth.Commands;

public static class VerifyEmail
{
    public class Command : IRequest
    {
        public string VerifyToken { get; set; }
    }

    internal sealed class Handler(VerifyEmailService verifyEmailService, ApplicationDbContext context)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var tokenContents = verifyEmailService.DecryptVerifyToken(request.VerifyToken);

            if (tokenContents == null)
                throw new BadRequestException("Hibás verify token");

            var user = await context.Users.FindAsync(tokenContents.UserId, cancellationToken);

            if (user == null)
                throw new NotFoundException();

            if (user.EmailVerifiedAt != null)
                throw new BadRequestException("Ez a felhasználó már megerősítette az email címét.");

            user.EmailVerifiedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}