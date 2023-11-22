using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.Shop.Commands;

public static class DeleteProduct
{
    public class Command : IRequest
    {
        public int Id { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, IPublisher publisher) : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await context.Products.FindAsync(request.Id);
            if (product == null) throw new NotFoundException(nameof(Product), request.Id);

            context.Products.Remove(product);
            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new ProductsUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}