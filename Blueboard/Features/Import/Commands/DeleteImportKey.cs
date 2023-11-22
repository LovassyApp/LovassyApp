using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.Import.Commands;

public static class DeleteImportKey
{
    public class Command : IRequest
    {
        public int Id { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var importKey = await context.ImportKeys.FindAsync(request.Id);

            if (importKey is null)
                throw new NotFoundException(nameof(ImportKey), request.Id);

            context.ImportKeys.Remove(importKey);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}