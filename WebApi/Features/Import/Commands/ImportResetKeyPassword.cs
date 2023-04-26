using FluentValidation;
using MediatR;
using WebApi.Core.Cryptography.Services;

namespace WebApi.Features.Import.Commands;

public class ImportResetKeyPasswordCommand : IRequest
{
    public ImportResetKeyPasswordBody Body { get; set; }
}

public class ImportResetKeyPasswordBody
{
    public string ResetKeyPassword { get; set; }
}

public class ImportResetKeyPasswordBodyValidator : AbstractValidator<ImportResetKeyPasswordBody>
{
    public ImportResetKeyPasswordBodyValidator()
    {
        RuleFor(x => x.ResetKeyPassword).NotEmpty();
    }
}

internal sealed class ImportResetKeyPasswordCommandHandler : IRequestHandler<ImportResetKeyPasswordCommand>
{
    private readonly ResetService _resetService;

    public ImportResetKeyPasswordCommandHandler(ResetService resetService)
    {
        _resetService = resetService;
    }

    public async Task<Unit> Handle(ImportResetKeyPasswordCommand request, CancellationToken cancellationToken)
    {
        _resetService.SetResetKeyPassword(request.Body.ResetKeyPassword);

        return Unit.Value;
    }
}