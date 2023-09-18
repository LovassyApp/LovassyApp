using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;

namespace Blueboard.Features.Shop.Commands;

public static class DeleteQRCode
{
    public class Commnad : IRequest
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Commnad>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublisher _publisher;

        public Handler(ApplicationDbContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(Commnad request, CancellationToken cancellationToken)
        {
            var qrcode = await _context.QRCodes.FindAsync(request.Id);

            if (qrcode == null) throw new NotFoundException(nameof(QRCode), request.Id);

            _context.QRCodes.Remove(qrcode);

            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new QRCodesUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}