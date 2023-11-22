using Blueboard.Core.Auth.Services;
using MediatR;

namespace Blueboard.Features.Auth.Commands;

public static class Logout
{
    public class Command : IRequest
    {
    }

    internal sealed class Handler(SessionManager sessionManager) : IRequestHandler<Command>
    {
        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            sessionManager.StopSession();

            return Unit.Task;
        }
    }
}