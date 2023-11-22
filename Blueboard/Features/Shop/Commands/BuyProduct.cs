using Blueboard.Core.Auth.Services;
using Blueboard.Core.Lolo.Exceptions;
using Blueboard.Core.Lolo.Services;
using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Shop.Commands;

public static class BuyProduct
{
    public class Command : IRequest
    {
        public int ProductId { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, LoloManager loloManager, UserAccessor userAccessor,
            IPublisher publisher)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await context.Products.FindAsync(request.ProductId);

            if (product == null) throw new NotFoundException(nameof(Product), request.ProductId);

            await loloManager.LoadAsync();

            if (product.Quantity < 1) throw new BadRequestException("A termék elfogyott.");

            if (loloManager.Balance < product.Price)
                throw new BadRequestException("Nincs elég lolód a termék megvásárlásához.");

            if (product.UserLimited)
            {
                var storeHistories = await context.StoreHistories
                    .Where(h => h.ProductId == product.Id && h.UserId == userAccessor.User.Id)
                    .ToListAsync(cancellationToken);

                if (storeHistories.Count >= product.UserLimit)
                    throw new BadRequestException("Ebből a termékből te nem vehetsz többet.");
            }

            var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await loloManager.SpendAsync(product.Price, false);
                product.Quantity--;

                var ownedItem = new OwnedItem
                {
                    UserId = userAccessor.User.Id,
                    ProductId = product.Id,
                    StoreHistory = new StoreHistory
                    {
                        UserId = userAccessor.User.Id,
                        ProductId = product.Id,
                        LolosSpent =
                            product.Price, // We don't want to trust the price from the product because it can change
                        Reason = $"Bazárban vásárolva - {product.Name}"
                    }
                };

                await context.OwnedItems.AddAsync(ownedItem, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                await publisher.Publish(new ProductsUpdatedEvent(), cancellationToken);
                await publisher.Publish(new LolosUpdatedEvent
                {
                    UserId = userAccessor.User.Id
                }, cancellationToken);
                await publisher.Publish(new OwnedItemUpdatedEvent
                {
                    UserId = ownedItem.UserId
                }, cancellationToken);
            }
            catch (InsufficientFundsException)
            {
                throw new BadRequestException("Nincs elég lolód a termék megvásárlásához.");
            }

            return Unit.Value;
        }
    }
}