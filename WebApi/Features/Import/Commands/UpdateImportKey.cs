using FluentValidation;
using Mapster;
using MediatR;
using WebApi.Common.Exceptions;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Import.Commands;

public class UpdateImportKeyCommand : IRequest
{
    public int Id { get; set; }
    public UpdateImportKeyBody Body { get; set; }
}

public class UpdateImportKeyBody
{
    public string Name { get; set; }
    public bool Enabled { get; set; }
}

public class UpdateImportKeyCommandValidator : AbstractValidator<UpdateImportKeyCommand>
{
    public UpdateImportKeyCommandValidator()
    {
        RuleFor(x => x.Body.Name).NotEmpty();
        RuleFor(x => x.Body.Enabled).NotNull();
    }
}

internal sealed class UpdateImportKeyCommandHandler : IRequestHandler<UpdateImportKeyCommand>
{
    private readonly ApplicationDbContext _context;

    public UpdateImportKeyCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateImportKeyCommand request, CancellationToken cancellationToken)
    {
        var importKey = await _context.ImportKeys.FindAsync(request.Id);

        if (importKey is null)
            throw new NotFoundException(nameof(ImportKey), request.Id);

        request.Body.Adapt(importKey);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}