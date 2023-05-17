using Helpers.Framework.Exceptions;
using MediatR;
using WebApi.Features.Auth.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Auth.Commands;

public static class VerifyEmail
{
    public class Command : IRequest
    {
        public string VerifyToken { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly VerifyEmailService _verifyEmailService;

        public Handler(VerifyEmailService verifyEmailService, ApplicationDbContext context)
        {
            _verifyEmailService = verifyEmailService;
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var tokenContents = _verifyEmailService.DecryptVerifyToken(request.VerifyToken);

            if (tokenContents is null)
                throw new BadRequestException("Invalid verify token");

            var user = await _context.Users.FindAsync(tokenContents.UserId, cancellationToken);

            if (user is null)
                throw new NotFoundException();

            if (user.EmailVerifiedAt is not null)
                throw new BadRequestException("Email already verified");

            user.EmailVerifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}