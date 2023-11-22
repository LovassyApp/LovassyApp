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

    internal sealed class Handler(ApplicationDbContext context, IPublisher publisher) : IRequestHandler<Commnad>
    {
        public async Task<Unit> Handle(Commnad request, CancellationToken cancellationToken)
        {
            var qrcode = await context.QRCodes.FindAsync(request.Id);

            if (qrcode == null) throw new NotFoundException(nameof(QRCode), request.Id);

            context.QRCodes.Remove(qrcode);

            await context.SaveChangesAsync(cancellationToken);

            await publisher.Publish(new QRCodesUpdatedEvent(), cancellationToken);

            return Unit.Value;
        }
    }
}