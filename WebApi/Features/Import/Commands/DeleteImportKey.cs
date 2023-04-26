using MediatR;
using WebApi.Common.Exceptions;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Import.Commands;

public class DeleteImportKeyCommand : IRequest
{
    public int Id { get; set; }
}

internal sealed class DeleteImportKeyCommandHandler : IRequestHandler<DeleteImportKeyCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteImportKeyCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteImportKeyCommand request, CancellationToken cancellationToken)
    {
        var importKey = await _context.ImportKeys.FindAsync(request.Id);

        if (importKey is null)
            throw new NotFoundException(nameof(ImportKey), request.Id);

        _context.ImportKeys.Remove(importKey);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}