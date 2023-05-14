using FluentValidation.Results;
using MediatR;
using WebApi.Common.Exceptions;
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
                throw new ValidationException(new[]
                    { new ValidationFailure(nameof(request.VerifyToken), "Invalid verify token") });

            var user = await _context.Users.FindAsync(tokenContents.UserId, cancellationToken);

            if (user is null)
                throw new NotFoundException();

            if (user.EmailVerifiedAt is not null)
                throw new ValidationException(new[]
                    { new ValidationFailure("User", "Email already verified") });

            user.EmailVerifiedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}