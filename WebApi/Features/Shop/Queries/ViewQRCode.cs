using System.Text.Json;
using Helpers.Framework.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Net.Codecrete.QrCodeGenerator;
using WebApi.Features.Shop.Models;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Shop.Queries;

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
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var qrcode = await _context.QRCodes.AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

            if (qrcode == null)
                throw new NotFoundException(nameof(QRCode), request.Id);

            var response = qrcode.Adapt<Response>();

            var svgString = QrCode.EncodeText(JsonSerializer.Serialize(new QRCodeImageContent
            {
                Id = qrcode.Id,
                Secret = qrcode.Secret
            }), QrCode.Ecc.Medium).ToSvgString(1);

            //Example of using this on the frontend: <img src={`data:image/svg+xml;base64,${btoa(unescape(encodeURIComponent(imageSvg)))}`} />
            // /\ Kind of disgusting, but I tried to find a better way and couldn't
            response.ImageSvg = svgString.Substring(svgString.IndexOf("<svg", StringComparison.Ordinal))
                .Replace("\n", "").Replace("\t", "");

            return response;
        }
    }
}