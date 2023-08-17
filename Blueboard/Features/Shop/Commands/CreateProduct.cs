using Blueboard.Features.Shop.Events;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Blueboard.Infrastructure.Persistence.Entities.Owned;
using FluentValidation;
using Ganss.Xss;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Shop.Commands;

public static class CreateProduct
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string RichTextContent { get; set; }

        public bool Visible { get; set; }

        public bool QRCodeActivated { get; set; }
        [AdaptIgnore] public int[] QRCodes { get; set; }

        public int Price { get; set; }
        public int Quantity { get; set; }

        public List<ResponseInput> Inputs { get; set; }

        public string[] NotifiedEmails { get; set; }

        public string ThumbnailUrl { get; set; }
    }

    public class ResponseInput
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public string Label { get; set; }
    }

    public class RequestBody
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RichTextContent { get; set; }

        public bool Visible { get; set; }

        public bool QRCodeActivated { get; set; }
        [AdaptIgnore] public int[] QRCodes { get; set; }

        public int Price { get; set; }
        public int Quantity { get; set; }

        public List<RequestBodyInput> Inputs { get; set; }

        public string[] NotifiedEmails { get; set; }

        public string ThumbnailUrl { get; set; }
    }

    public class RequestBodyInput
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public string Label { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        private readonly ApplicationDbContext _context;

        public RequestBodyValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
            RuleFor(x => x.RichTextContent).NotEmpty();
            RuleFor(x => x.Visible).NotNull();
            RuleFor(x => x.QRCodeActivated).NotNull();
            RuleFor(x => x.QRCodes).NotNull().MustAsync(BeExistingQRCodes)
                .WithMessage("A megadott QR kódok közül legalább egy nem létezik.");
            RuleFor(x => x.Price).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Quantity).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Inputs).NotNull().Must(i => i.DistinctBy(e => e.Key).Count() == i.Count)
                .WithMessage("A megadott inputok között van két azonos kulcsú elem.");
            RuleForEach(x => x.Inputs).ChildRules(v =>
            {
                v.RuleFor(i => i.Key).NotEmpty().Matches("^[a-zA-Z][a-zA-Z0-9_]*$").WithMessage(
                    "A kulcs csak angol kis- és nagybetűket, számokat és alulvonásokat tartalmazhat, és nem kezdődhet számmal.");
                v.RuleFor(i => i.Label).NotEmpty();
                v.RuleFor(i => i.Type).NotEmpty().IsEnumName(typeof(ProductInputType));
            });
            RuleFor(x => x.NotifiedEmails).NotNull();
            RuleForEach(x => x.NotifiedEmails).EmailAddress();
            RuleFor(x => x.ThumbnailUrl).NotEmpty();
        }

        private async Task<bool> BeExistingQRCodes(RequestBody model, int[] qrCodes,
            CancellationToken cancellationToken)
        {
            return await _context.QRCodes.Select(c => c.Id)
                       .CountAsync(id => qrCodes.Contains(id), cancellationToken) ==
                   qrCodes.Distinct().Count();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublisher _publisher;

        public Handler(ApplicationDbContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = request.Body.Adapt<Product>();

            var htmlSanitizer = new HtmlSanitizer();
            product.RichTextContent = htmlSanitizer.Sanitize(request.Body.RichTextContent);

            product.QRCodes = new List<QRCode>();

            foreach (var codeId in request.Body.QRCodes)
            {
                var qrcode = new QRCode
                {
                    Id = codeId
                };
                _context.Entry(qrcode).State = EntityState.Unchanged;
                product.QRCodes.Add(qrcode);
            }

            await _context.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new ProductUpdatedEvent(), cancellationToken);

            var response = product.Adapt<Response>();
            response.QRCodes = request.Body.QRCodes;

            return response;
        }
    }
}