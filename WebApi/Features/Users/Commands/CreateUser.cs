using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;
using WebApi.Common.Exceptions;
using WebApi.Core.Cryptography.Models;
using WebApi.Core.Cryptography.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Users.Commands;

public static class CreateUser
{
    public class Command : IRequest
    {
        public RequestBody Body { get; set; }
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

            RuleFor(x => x.Email).NotEmpty().EmailAddress()
                .Must(EndWithAllowedDomainEmail).WithMessage("The email must end with '@lovassy.edu.hu'")
                .MustAsync(BeUniqueEmail).WithMessage("The email is already in use");
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.OmCode).NotEmpty()
                .MustAsync(BeUniqueOmCode).WithMessage("The om code is already in use");
        }

        private bool EndWithAllowedDomainEmail(RequestBody model, string email)
        {
            return email.EndsWith("@lovassy.edu.hu");
        }

        private Task<bool> BeUniqueEmail(RequestBody model, string email, CancellationToken cancellationToken)
        {
            return _context.Users.AllAsync(x => x.Email != email, cancellationToken);
        }

        private Task<bool> BeUniqueOmCode(RequestBody model, string omCode, CancellationToken cancellationToken)
        {
            return _context.Users.AllAsync(x => x.OmCodeHashed != _hashService.Hash(omCode), cancellationToken);
        }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly EncryptionManager _encryptionManager;
        private readonly HashService _hashService;
        private readonly ResetService _resetService;

        public Handler(ApplicationDbContext context, EncryptionManager encryptionManager,
            HashService hashService, ResetService resetService)
        {
            _context = context;
            _encryptionManager = encryptionManager;
            _hashService = hashService;
            _resetService = resetService;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!_resetService.IsResetKeyPasswordSet()) throw new UnavailableException();

            var masterKeySalt = _hashService.GenerateSalt();
            var masterKey = new EncryptableKey();
            _encryptionManager.Init(masterKey.GetKey());

            var storedKey = masterKey.Lock(request.Body.Password, masterKeySalt);

            var keys = new KyberKeypair();

            var hasherSalt = _hashService.GenerateSalt();

            var user = new User
            {
                Email = request.Body.Email,
                Name = request.Body.Name,
                PasswordHashed = _hashService.HashPassword(request.Body.Password),
                PublicKey = keys.PublicKey,
                PrivateKeyEncrypted = _encryptionManager.Encrypt(keys.PrivateKey),
                MasterKeyEncrypted = storedKey,
                MasterKeySalt = masterKeySalt,
                ResetKeyEncrypted = _resetService.EncryptMasterKey(_encryptionManager.MasterKey!),
                HasherSaltEncrypted = _encryptionManager.Encrypt(hasherSalt),
                HasherSaltHashed = _hashService.Hash(hasherSalt),
                OmCodeEncrypted = _encryptionManager.Encrypt(request.Body.OmCode),
                OmCodeHashed = _hashService.Hash(request.Body.OmCode)
            };

            user.DomainEvents.Add(new UserCreatedEvent(user));

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

public class UserCreatedEvent : DomainEvent //TODO: Add user to default group in a handler
{
    public UserCreatedEvent(User user)
    {
        User = user;
    }

    public User User { get; set; }
}