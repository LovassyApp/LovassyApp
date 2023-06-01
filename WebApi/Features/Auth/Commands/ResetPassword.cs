using System.Security.Cryptography;
using FluentValidation;
using Helpers.Cryptography.Services;
using Helpers.Cryptography.Utils;
using Helpers.Framework.Exceptions;
using MediatR;
using WebApi.Core.Auth.Services;
using WebApi.Features.Auth.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Auth.Commands;

public static class ResetPassword
{
    public class Command : IRequest
    {
        public string PasswordResetToken { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string NewPassword { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty()
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$").WithMessage(
                    "The password must contain at least one uppercase letter, lower case letter, number and must be at least 8 characters long");
        }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly HashService _hashService;
        private readonly PasswordResetService _passwordResetService;
        private readonly ResetService _resetService;

        public Handler(ApplicationDbContext context, HashService hashService,
            PasswordResetService passwordResetService, ResetService resetService)
        {
            _context = context;
            _hashService = hashService;
            _passwordResetService = passwordResetService;
            _resetService = resetService;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!_resetService.IsResetKeyPasswordSet())
                throw new UnavailableException("Reset key password is not yet set");


            var tokenContents = _passwordResetService.DecryptPasswordResetToken(request.PasswordResetToken);


            if (tokenContents == null)
                throw new BadRequestException("Invalid password reset token");

            var user = await _context.Users.FindAsync(tokenContents.UserId, cancellationToken);

            if (user == null)
                throw new NotFoundException();

            string resetKey;

            try
            {
                resetKey = _resetService.DecryptMasterKey(user.ResetKeyEncrypted, user.MasterKeySalt);
            }
            catch (CryptographicException)
            {
                throw new UnavailableException(
                    "An invalid reset key password was set, the password reset feature will remain unavailable until the old reset key password is set again (Please contact a system administrator to resolve this issue)");
            }

            var newMasterKeyEncrypted = EncryptionUtils.Encrypt(resetKey,
                HashingUtils.GenerateBasicKey(request.Body.NewPassword, user.MasterKeySalt));

            user.MasterKeyEncrypted = newMasterKeyEncrypted;
            user.PasswordHashed = _hashService.HashPassword(request.Body.NewPassword);

            await _context.SaveChangesAsync(cancellationToken);

            //TODO: Send out an event and a notification through websockets in the handler

            return Unit.Value;
        }
    }
}