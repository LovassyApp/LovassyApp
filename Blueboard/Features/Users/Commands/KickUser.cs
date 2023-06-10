using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Users.Commands;

public static class KickUser
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly SessionService _sessionService;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, SessionService sessionService, UserAccessor userAccessor)
        {
            _context = context;
            _sessionService = sessionService;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.Id == _userAccessor.User.Id) throw new BadRequestException("You cannot kick yourself");

            var user = await _context.Users.Where(u => u.Id == request.Id).Include(u => u.PersonalAccessTokens)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null) throw new NotFoundException(nameof(User), request.Id);

            foreach (var token in user.PersonalAccessTokens) _sessionService.StopSession(token.Token);

            _context.PersonalAccessTokens.RemoveRange(user.PersonalAccessTokens);

            await _context.SaveChangesAsync(cancellationToken);

            //TODO: Send out a notification to the user that they have been kicked

            return Unit.Value;
        }
    }
}