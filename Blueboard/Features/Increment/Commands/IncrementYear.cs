using Blueboard.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Increment.Commands;

public static class IncrementYear
{
    public class Command : IRequest
    {
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string ConfirmationText { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        // For future developers (who probably don't exist): Please accept my condolences for having to work on this project.
        
        private const string ExpectedConfirmationText = "Mit sütsz, kis szűcs? Sós húst sütsz, kis szűcs?";
        public RequestBodyValidator()
        {
            RuleFor(x => x.ConfirmationText)
                .NotEmpty()
                .WithMessage("A megerősítő szöveg nem lehet üres.");
            
            RuleFor(x => x.ConfirmationText)
                .Must(s=> string.Equals(s?.Trim(), ExpectedConfirmationText, StringComparison.Ordinal))
                .When(x => !string.IsNullOrWhiteSpace(x.ConfirmationText))
                .WithMessage("A megerősítő szöveg nem egyezik.");
        }    
    }

    internal sealed class Handler: IRequestHandler<Command, Unit>
    {
        private readonly ApplicationDbContext _context;
        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            await _context.GradeImports.ExecuteDeleteAsync(cancellationToken);
            await _context.LoloRequests.ExecuteDeleteAsync(cancellationToken);
            await _context.Lolos.ExecuteDeleteAsync(cancellationToken);
            await _context.OwnedItemUses.ExecuteDeleteAsync(cancellationToken);
            await _context.StoreHistories.ExecuteDeleteAsync(cancellationToken);
            await _context.OwnedItems.ExecuteDeleteAsync(cancellationToken);
            await _context.Grades.ExecuteDeleteAsync(cancellationToken);

            await _context.Products.ExecuteDeleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}