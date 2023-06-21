using Blueboard.Core.Auth.Services;
using Blueboard.Features.Auth.Events;
using Blueboard.Features.Auth.Models;
using Blueboard.Features.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Helpers.Cryptography.Implementations;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;

namespace Blueboard.Features.Auth.Commands;

public static class Refresh
{
    public class Command : IRequest<Response>
    {
        public string? RefreshToken { get; set; }
    }

    public class Response
    {
        public ResponseUser User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
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
        private readonly IPublisher _publisher;
        private readonly RefreshService _refreshService;
        private readonly SessionManager _sessionManager;

        public Handler(IPublisher publisher, ApplicationDbContext context,
            SessionManager sessionManager, EncryptionManager encryptionManager, RefreshService refreshService)
        {
            _publisher = publisher;
            _context = context;
            _sessionManager = sessionManager;
            _encryptionManager = encryptionManager;
            _refreshService = refreshService;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.RefreshToken is null)
                throw new BadRequestException("A 'refreshToken' vagy a refresh süti hiányzik.");

            RefreshTokenContents? refreshTokenContents;
            try
            {
                refreshTokenContents = _refreshService.DecryptRefreshToken(request.RefreshToken);
            }
            catch
            {
                throw new BadRequestException("Hibás refresh token.");
            }

            if (refreshTokenContents is null)
                throw new BadRequestException("Hibás refresh token.");

            // I wouldn't risk as no tracking here as the user might be updated by the update grades job (although it's reattached there)
            var user = await _context.Users.FindAsync(refreshTokenContents.UserId);

            if (user == null)
                throw new BadRequestException("Hibás refresh token.");

            var token = await _sessionManager.StartSessionAsync(user.Id);

            var masterKey = new EncryptableKey(user.MasterKeyEncrypted);
            var unlockedMasterKey = masterKey.Unlock(refreshTokenContents.Password, user.MasterKeySalt);

            _encryptionManager.MasterKey = unlockedMasterKey; // Set the master key this way saves it in the session

            var refreshToken = _refreshService.GenerateRefreshToken(user.Id, refreshTokenContents.Password);

            await _publisher.Publish(new SessionCreatedEvent
            {
                User = user,
                MasterKey = unlockedMasterKey
            }, cancellationToken);

            return new Response
            {
                User = user.Adapt<ResponseUser>(),
                Token = token,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = DateTime.Now.Add(_refreshService.GetRefreshTokenExpiry())
            };
        }
    }
}