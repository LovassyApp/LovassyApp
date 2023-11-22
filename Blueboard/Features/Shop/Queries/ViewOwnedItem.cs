using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Shop.Queries;

public static class ViewOwnedItem
{
    public class Query : IRequest<Response>
    {
        public int Id { get; set; }
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

    internal sealed class Handler(ApplicationDbContext context, PermissionManager permissionManager,
            UserAccessor userAccessor)
        : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var ownedItem = await context.OwnedItems.Include(i => i.Product).AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (ownedItem == null)
                throw new NotFoundException(nameof(OwnedItem), request.Id);

            if (ownedItem.UserId != userAccessor.User.Id &&
                !permissionManager.CheckPermission(typeof(ShopPermissions.ViewOwnedItem)))
                throw new NotFoundException(nameof(OwnedItem), request.Id);

            return ownedItem.Adapt<Response>();
        }
    }
}