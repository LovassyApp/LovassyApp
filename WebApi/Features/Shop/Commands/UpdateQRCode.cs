using FluentValidation;
using Helpers.Framework.Exceptions;
using Mapster;
using MediatR;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Shop.Commands;

public static class UpdateQRCode
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var qrcode = await _context.QRCodes.FindAsync(request.Id);

            if (qrcode == null) throw new NotFoundException(nameof(QRCode), request.Id);

            request.Body.Adapt(qrcode);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}