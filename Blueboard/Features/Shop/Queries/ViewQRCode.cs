using System.Text.Json;
using Blueboard.Features.Shop.Models;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Net.Codecrete.QrCodeGenerator;

namespace Blueboard.Features.Shop.Queries;

public static class ViewQRCode
{
    public class Query : IRequest<Response>
    {
        public int Id { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageSvg { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var qrcode = await context.QRCodes.AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

            if (qrcode == null)
                throw new NotFoundException(nameof(QRCode), request.Id);

            var response = qrcode.Adapt<Response>();

            var svgString = QrCode.EncodeText(JsonSerializer.Serialize(new QRCodeImageContent
            {
                Id = qrcode.Id,
                Secret = qrcode.Secret
            }), QrCode.Ecc.Medium).ToSvgString(1);

            // Example of using this on the frontend: <img src={`data:image/svg+xml;base64,${btoa(unescape(encodeURIComponent(imageSvg)))}`} />
            // /\ Kind of disgusting, but I tried to find a better way and couldn't
            response.ImageSvg = svgString.Substring(svgString.IndexOf("<svg", StringComparison.Ordinal))
                .Replace("\n", "").Replace("\t", "");

            return response;
        }
    }
}