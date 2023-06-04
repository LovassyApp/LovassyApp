using Blueboard.Core.Auth.Services;
using MediatR;

namespace Blueboard.Features.Auth.Commands;

public static class Logout
{
    public class Command : IRequest
    {
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly SessionManager _sessionManager;

        public Handler(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            _sessionManager.StopSession();

            return Unit.Task;
        }
    }
}