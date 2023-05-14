using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Models;
using WebApi.Core.Cryptography.Services;
using WebApi.Features.Auth.Events;
using WebApi.Features.Auth.Services;
using WebApi.Infrastructure.Persistence;
using ValidationException = WebApi.Common.Exceptions.ValidationException;

namespace WebApi.Features.Auth.Commands;

public static class Login
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        private readonly ApplicationDbContext _context;

        public RequestBodyValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Email).NotEmpty().EmailAddress()
                .MustAsync(BeExistingUsersEmailAsync).WithMessage("The email is not yet registered");
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Remember).NotNull();
        }

        private async Task<bool> BeExistingUsersEmailAsync(RequestBody model, string email,
            CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }
    }

    public class Response
    {
        public ResponseUser User { get; set; }
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
    }

    public class ResponseUser
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public string? RealName { get; set; }
        public string? Class { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly EncryptionManager _encryptionManager;
        private readonly HashService _hashService;
        private readonly IPublisher _publisher;
        private readonly RefreshService _refreshService;
        private readonly SessionManager _sessionManager;

        public Handler(ApplicationDbContext context, HashService hashService, EncryptionManager encryptionManager,
            IPublisher publisher, RefreshService refreshService, SessionManager sessionManager)
        {
            _context = context;
            _publisher = publisher;
            _hashService = hashService;
            _encryptionManager = encryptionManager;
            _refreshService = refreshService;
            _sessionManager = sessionManager;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(x => x.Email == request.Body.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (!_hashService.VerifyPassword(request.Body.Password, user!.PasswordHashed))
                throw new ValidationException(new[]
                    { new ValidationFailure(nameof(request.Body.Password), "Invalid password") });

            var token = await _sessionManager.StartSessionAsync(user.Id);

            var masterKey = new EncryptableKey(user.MasterKeyEncrypted);
            var unlockedMasterKey = masterKey.Unlock(request.Body.Password, user.MasterKeySalt);

            _encryptionManager.MasterKey = unlockedMasterKey; // Setting the master key this way saves it in the session

            await _publisher.Publish(new SessionCreatedEvent
            {
                User = user,
                MasterKey = unlockedMasterKey
            }, cancellationToken);

            if (request.Body.Remember)
            {
                var refreshToken = _refreshService.GenerateRefreshToken(user.Id, request.Body.Password);

                return new Response
                {
                    User = user.Adapt<ResponseUser>(),
                    Token = token,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiration = DateTime.Now.Add(_refreshService.GetRefreshTokenExpiry())
                };
            }

            return new Response
            {
                User = user.Adapt<ResponseUser>(),
                Token = token
            }; //TODO: Maybe add warden permissions to response
        }
    }
}