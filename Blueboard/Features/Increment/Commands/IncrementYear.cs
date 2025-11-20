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
        public RequestBodyValidator()
        {
            RuleFor(x => x.ConfirmationText).NotEmpty().NotNull();
        }    
    }

    internal sealed class Handler: IRequestHandler<Command, Unit>
    {
        private readonly ApplicationDbContext _context;

        private readonly IMediator _mediator;
        
        public Handler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request?.Body.ConfirmationText == null)
            {
                throw new ValidationException("A megerősítő szöveg nem lehet üres.");
            }
            
            if (!string.Equals(request.Body.ConfirmationText, "Mit sütsz, kis szűcs? Sós húst sütsz, kis szűcs?"))
            {
                throw new ValidationException("A megerősítő szöveg nem egyezik.");
            }
            
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