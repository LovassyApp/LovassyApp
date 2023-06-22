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

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly IPublisher _publisher;
        private readonly ResetService _resetService;

        public Handler(ResetService resetService, IPublisher publisher)
        {
            _resetService = resetService;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            // It's fairly safe to do this before setting the reset key password, because all that does is set a string data member
            if (!_resetService.IsResetKeyPasswordSet())
                await _publisher.Publish(new ResetKeyPasswordSetEvent(), cancellationToken);

            _resetService.SetResetKeyPassword(request.Body.ResetKeyPassword);

            return Unit.Value;
        }
    }
}