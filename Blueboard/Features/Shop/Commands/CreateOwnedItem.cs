using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Shop.Commands;

public static class CreateOwnedItem
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public ResponseProduct Product { get; set; }

        public DateTime? UsedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseProduct
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string RichTextContent { get; set; }

        public bool QRCodeActivated { get; set; }

        public List<ResponseProductInput> Inputs { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseProductInput
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public string Label { get; set; }
    }

    public class RequestBody
    {
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime? UsedAt { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        private readonly ApplicationDbContext _context;

        public RequestBodyValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.UserId).NotEmpty().MustAsync(BeExistingUserAsync)
                .WithMessage("Nem létezik felhasználó a megadott azonosítóval");
            RuleFor(x => x.ProductId).NotEmpty().MustAsync(BeExistingProductAsync)
                .WithMessage("Nem létezik termék a megadott azonosítóval");
        }

        private async Task<bool> BeExistingUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x => x.Id == userId, cancellationToken);
        }

        private async Task<bool> BeExistingProductAsync(int productId, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(x => x.Id == productId, cancellationToken);
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FindAsync(request.Body.ProductId);

            var ownedItem = request.Body.Adapt<OwnedItem>();
            ownedItem.Product = product!;

            await _context.OwnedItems.AddAsync(ownedItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return ownedItem.Adapt<Response>();
        }
    }
}