using Blueboard.Core.Auth.Services;
using Blueboard.Features.Import.Events;
using FluentValidation;
using MediatR;

namespace Blueboard.Features.Import.Commands;

public static class UpdateResetKeyPassword
{
    public class Command : IRequest
    {
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string ResetKeyPassword { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.ResetKeyPassword).NotEmpty();
        }
    }

    internal sealed class Handler(ResetService resetService, IPublisher publisher) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            // It's fairly safe to do this before setting the reset key password, because all that does is set a string data member
            if (!resetService.IsResetKeyPasswordSet())
                await publisher.Publish(new ResetKeyPasswordSetEvent(), cancellationToken);

            resetService.SetResetKeyPassword(request.Body.ResetKeyPassword);

            return Unit.Value;
        }
    }
}