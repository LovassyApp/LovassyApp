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

            if (tokenContents == null)
                throw new BadRequestException("Invalid verify token");

            var user = await _context.Users.FindAsync(tokenContents.UserId, cancellationToken);

            if (user == null)
                throw new NotFoundException();

            if (user.EmailVerifiedAt != null)
                throw new BadRequestException("Email already verified");

            user.EmailVerifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}