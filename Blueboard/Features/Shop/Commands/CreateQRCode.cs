using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Mapster;
using MediatR;
using Org.BouncyCastle.Security;

namespace Blueboard.Features.Shop.Commands;

public static class CreateQRCode
{
    public class Command : IRequest<Response>
    {
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

    public class Response
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var qrCode = request.Body.Adapt<QRCode>();

            var secureRandom = new SecureRandom();
            var secretBytes = new byte[24];
            secureRandom.NextBytes(secretBytes);

            qrCode.Secret = Convert.ToBase64String(secretBytes);

            await _context.QRCodes.AddAsync(qrCode, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return qrCode.Adapt<Response>();
        }
    }
}