using FluentValidation;
using Mapster;
using MediatR;
using WebApi.Common.Exceptions;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Import.Commands;

public static class UpdateImportKey
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Enabled).NotNull();
        }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var importKey = await _context.ImportKeys.FindAsync(request.Id);

            if (importKey is null)
                throw new NotFoundException(nameof(ImportKey), request.Id);

            request.Body.Adapt(importKey);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}