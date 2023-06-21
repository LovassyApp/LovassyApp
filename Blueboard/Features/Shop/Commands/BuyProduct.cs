using Blueboard.Core.Auth.Services;
using Blueboard.Core.Lolo.Exceptions;
using Blueboard.Core.Lolo.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.Shop.Commands;

public static class BuyProduct
{
    public class Command : IRequest
    {
        public int ProductId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly LoloManager _loloManager;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, LoloManager loloManager, UserAccessor userAccessor)
        {
            _context = context;
            _loloManager = loloManager;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId);

            if (product == null) throw new NotFoundException(nameof(Product), request.ProductId);

            await _loloManager.LoadAsync();

            if (product.Quantity < 1) throw new BadRequestException("A termék elfogyott.");

            if (_loloManager.Balance < product.Price)
                throw new BadRequestException("Nincs elég lolód a termék megvásárlásához.");

            var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await _loloManager.SpendAsync(product.Price, false);
                product.Quantity--;

                var ownedItem = new OwnedItem
                {
                    UserId = _userAccessor.User.Id,
                    ProductId = product.Id,
                    StoreHistory = new StoreHistory
                    {
                        UserId = _userAccessor.User.Id,
                        ProductId = product.Id,
                        LolosSpent =
                            product.Price, // We don't want to trust the price from the product because it can change
                        Reason = $"Bazárban vásárolva - {product.Name}"
                    }
                };

                await _context.OwnedItems.AddAsync(ownedItem, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (InsufficientFundsException)
            {
                throw new BadRequestException("Nincs elég lolód a termék megvásárlásához.");
            }

            //TODO: Send notification to user about a new owned item and to all users about the quantity update

            return Unit.Value;
        }
    }
}