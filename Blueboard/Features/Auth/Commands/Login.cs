using Blueboard.Core.Auth.Services;
using Blueboard.Features.Auth.Events;
using Blueboard.Features.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.Results;
using Helpers.Cryptography.Implementations;
using Helpers.Cryptography.Services;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ValidationException = Helpers.WebApi.Exceptions.ValidationException;

namespace Blueboard.Features.Auth.Commands;

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

            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255)
                .MustAsync(BeExistingUsersEmailAsync).WithMessage("Nem létezik ilyen email című felhasználó.");
            RuleFor(x => x.Password).NotEmpty().MaximumLength(255);
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

    internal sealed class Handler(ApplicationDbContext context, HashService hashService,
            EncryptionManager encryptionManager,
            IPublisher publisher, RefreshService refreshService, SessionManager sessionManager)
        : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            // I wouldn't risk as no tracking here as the user might be updated by the update grades job (although it's reattached there)
            var user = await context.Users.Where(x => x.Email == request.Body.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (!hashService.VerifyPassword(request.Body.Password, user!.PasswordHashed))
                throw new ValidationException(new[]
                    { new ValidationFailure(nameof(request.Body.Password), "A megadott jelszó hibás.") });

            var token = await sessionManager.StartSessionAsync(user.Id);

            var masterKey = new EncryptableKey(user.MasterKeyEncrypted);
            var unlockedMasterKey = masterKey.Unlock(request.Body.Password, user.MasterKeySalt);

            encryptionManager.MasterKey = unlockedMasterKey; // Setting the master key this way saves it in the session

            await publisher.Publish(new SessionCreatedEvent
            {
                User = user,
                MasterKey = unlockedMasterKey
            }, cancellationToken);

            if (request.Body.Remember)
            {
                var refreshToken = refreshService.GenerateRefreshToken(user.Id, request.Body.Password);

                return new Response
                {
                    User = user.Adapt<ResponseUser>(),
                    Token = token,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiration = DateTime.Now.Add(refreshService.GetRefreshTokenExpiry())
                };
            }

            return new Response
            {
                User = user.Adapt<ResponseUser>(),
                Token = token
            };
        }
    }
}