using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Shop.Queries;

public static class IndexProducts
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
        public string? Search { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Visible { get; set; }

        public bool QRCodeActivated { get; set; }

        public int Price { get; set; }
        public int Quantity { get; set; }

        public bool UserLimited { get; set; }
        public int UserLimit { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, SieveProcessor sieveProcessor,
            PermissionManager permissionManager)
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var hasIndexProductsPermission = permissionManager.CheckPermission(typeof(ShopPermissions.IndexProducts));

            var products = context.Products.AsNoTracking();

            var filteredProducts = sieveProcessor.Apply(request.SieveModel, products);

            if (!hasIndexProductsPermission) filteredProducts = filteredProducts.Where(p => p.Visible);

            if (!string.IsNullOrEmpty(request.Search))
            {
                var searchQuery = string.Join(" & ", request.Search.Split(" ").Select(w => w + ":*"));
                filteredProducts = filteredProducts.Where(p =>
                    p.SearchVector.Matches(EF.Functions.ToTsQuery("hungarian", searchQuery)));
            }

            return (await filteredProducts.ToListAsync(cancellationToken)).Adapt<IEnumerable<Response>>();
        }
    }
}