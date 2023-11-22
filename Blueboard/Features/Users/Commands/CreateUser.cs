using Blueboard.Core.Auth.Services;
using Blueboard.Features.Users.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.Cryptography.Implementations;
using Helpers.Cryptography.Services;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Users.Commands;

public static class CreateUser
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
        public string VerifyUrl { get; set; }
        public string VerifyTokenQueryKey { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public string? RealName { get; set; }
        public string? Class { get; set; }
    }

    public class RequestBody
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }
        public string OmCode { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        private readonly ApplicationDbContext _context;
        private readonly HashService _hashService;

        public RequestBodyValidator(ApplicationDbContext context, HashService hashService)
        {
            _context = context;
            _hashService = hashService;

            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255)
                .Must(EndWithAllowedDomainEmail)
                .WithMessage("Az email cím '@lovassy.edu.hu'-ra kell hogy végződjön.")
                .MustAsync(BeUniqueEmailAsync).WithMessage("Az email cím már használatban van.");
            RuleFor(x => x.Password).NotEmpty()
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d\w\W]{8,}$").WithMessage(
                    "A jelszónak legalább 8 karakter hosszúnak kell lennie, és tartalmaznia kell legalább egy kisbetűt, egy nagybetűt és egy számot.");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.OmCode).NotEmpty()
                .MustAsync(BeUniqueOmCodeAsync).WithMessage("Az OM azonosító már használatban van.")
                .Matches(@"^\d{11}$").WithMessage("Az OM azonosítónak 11 számjegyből kell állnia.");
        }

        private bool EndWithAllowedDomainEmail(RequestBody model, string email)
        {
            return email.EndsWith("@lovassy.edu.hu");
        }

        private async Task<bool> BeUniqueEmailAsync(RequestBody model, string email,
            CancellationToken cancellationToken)
        {
            return await _context.Users.AllAsync(x => x.Email != email, cancellationToken);
        }

        private Task<bool> BeUniqueOmCodeAsync(RequestBody model, string omCode, CancellationToken cancellationToken)
        {
            return _context.Users.AllAsync(x => x.OmCodeHashed != _hashService.Hash(omCode), cancellationToken);
        }
    }

    internal sealed class Handler(IPublisher publisher, ApplicationDbContext context,
            EncryptionManager encryptionManager, HashService hashService, ResetService resetService)
        : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!resetService.IsResetKeyPasswordSet())
                throw new UnavailableException(
                    "A visszaállítási jelszó a LovassyApp legutóbbi újraindítása óta még nem lett beállítva.");

            var masterKeySalt = hashService.GenerateSalt();
            var masterKey = new EncryptableKey();
            encryptionManager.SetMasterKeyTemporarily(masterKey.GetKey());

            var storedKey = masterKey.Lock(request.Body.Password, masterKeySalt);

            var keys = new KyberKeypair();

            var hasherSalt = hashService.GenerateSalt();

            var user = new User
            {
                Email = request.Body.Email,
                Name = request.Body.Name,
                PasswordHashed = hashService.HashPassword(request.Body.Password),
                PublicKey = keys.PublicKey,
                PrivateKeyEncrypted = encryptionManager.Encrypt(keys.PrivateKey),
                MasterKeyEncrypted = storedKey,
                MasterKeySalt = masterKeySalt,
                ResetKeyEncrypted = resetService.EncryptMasterKey(encryptionManager.MasterKey!, masterKeySalt),
                HasherSaltEncrypted = encryptionManager.Encrypt(hasherSalt),
                HasherSaltHashed = hashService.Hash(hasherSalt),
                OmCodeEncrypted = encryptionManager.Encrypt(request.Body.OmCode),
                OmCodeHashed = hashService.Hash(request.Body.OmCode)
            };

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new UserCreatedEvent
            {
                User = user,
                VerifyUrl = request.VerifyUrl,
                VerifyTokenQueryKey = request.VerifyTokenQueryKey
            }, cancellationToken);

            return user.Adapt<Response>();
        }
    }
}