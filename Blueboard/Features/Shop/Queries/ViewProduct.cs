using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Shop.Queries;

public static class ViewProduct
{
    public class Query : IRequest<Response>
    {
        public int Id { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RichTextContent { get; set; }

        public bool Visible { get; set; }

        public bool QRCodeActivated { get; set; }
        public List<ResponseQRCode> QRCodes { get; set; }

        public int Price { get; set; }
        public int Quantity { get; set; }

        public bool UserLimited { get; set; }
        public int UserLimit { get; set; }

        public List<ResponseInput> Inputs { get; set; }

        public string[]? NotifiedEmails { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ResponseInput
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public string Label { get; set; }
    }

    public class ResponseQRCode
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly PermissionManager _permissionManager;

        public Handler(ApplicationDbContext context, PermissionManager permissionManager)
        {
            _context = context;
            _permissionManager = permissionManager;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Include(p => p.QRCodes)
                .Where(p => p.Id == request.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
                throw new NotFoundException(nameof(Product), request.Id);

            if (!_permissionManager.CheckPermission(typeof(ShopPermissions.ViewProduct)))
            {
                if (!product.Visible)
                    throw new NotFoundException(nameof(Product), request.Id);

                var response = product.Adapt<Response>();
                response.NotifiedEmails = null; // To keep the emails private

                return response;
            }

            return product.Adapt<Response>();
        }
    }
}