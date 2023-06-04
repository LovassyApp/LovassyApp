using Blueboard.Core.Auth.Services;
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
        private readonly ResetService _resetService;

        public Handler(ResetService resetService)
        {
            _resetService = resetService;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            _resetService.SetResetKeyPassword(request.Body.ResetKeyPassword);

            return Unit.Value;
        }
    }
}