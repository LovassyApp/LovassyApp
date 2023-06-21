using System.Security.Cryptography;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.Auth.Events;
using Blueboard.Features.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using FluentValidation;
using Helpers.Cryptography.Services;
using Helpers.Cryptography.Utils;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.Auth.Commands;

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
                    "A jelszónak legalább 8 karakter hosszúnak kell lennie, tartalmaznia kell legalább egy kisbetűt, egy nagybetűt és egy számot.");
        }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly HashService _hashService;
        private readonly PasswordResetService _passwordResetService;
        private readonly IPublisher _publisher;
        private readonly ResetService _resetService;

        public Handler(ApplicationDbContext context, HashService hashService,
            PasswordResetService passwordResetService, ResetService resetService, IPublisher publisher)
        {
            _context = context;
            _hashService = hashService;
            _passwordResetService = passwordResetService;
            _resetService = resetService;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!_resetService.IsResetKeyPasswordSet())
                throw new UnavailableException(
                    "A visszaállítási jelszó a LovassyApp legutóbbi újraindítása óta még nem lett beállítva.");


            var tokenContents = _passwordResetService.DecryptPasswordResetToken(request.PasswordResetToken);


            if (tokenContents == null)
                throw new BadRequestException("Hibás reset token");

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
                    "Hibás visszaállítási jelszó lett beállítva, a jelszó visszaállítása funkció elérhetetlen marad amíg az eredeti visszaállítási jelszó be nem lesz állítva. (Kérlek vedd fel a kapcsolatot a fejlesztőkkel a hiba elhárításához)");
            }

            var newMasterKeyEncrypted = EncryptionUtils.Encrypt(resetKey,
                HashingUtils.GenerateBasicKey(request.Body.NewPassword, user.MasterKeySalt));

            user.MasterKeyEncrypted = newMasterKeyEncrypted;
            user.PasswordHashed = _hashService.HashPassword(request.Body.NewPassword);

            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new PasswordResetEvent
            {
                User = user
            }, cancellationToken);

            //TODO: Send out an event and a notification through websockets in the handler

            return Unit.Value;
        }
    }
}