using System.Text.Json;
using Blueboard.Core.Auth.Services;
using Blueboard.Features.Shop.Events;
using Blueboard.Features.Shop.Models;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Blueboard.Infrastructure.Persistence.Entities.Owned;
using FluentValidation.Results;
using Helpers.WebApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Features.Shop.Commands;

public static class UseOwnedItem
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string? QRCodeContent { get; set; }
        public Dictionary<string, string>? Inputs { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublisher _publisher;
        private readonly UserAccessor _userAccessor;

        public Handler(ApplicationDbContext context, UserAccessor userAccessor, IPublisher publisher)
        {
            _context = context;
            _userAccessor = userAccessor;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var ownedItem = await _context.OwnedItems.Include(i => i.Product).ThenInclude(p => p.QRCodes)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (ownedItem == null || ownedItem.UserId != _userAccessor.User.Id)
                throw new NotFoundException(nameof(OwnedItem), request.Id);

            if (ownedItem.UsedAt != null)
                throw new BadRequestException("Ezt a terméket már felhasználtad.");

            // Check if the QR code is valid and handle it
            string? qrCodeEmail = null;

            if (ownedItem.Product.QRCodeActivated)
            {
                if (string.IsNullOrEmpty(request.Body.QRCodeContent))
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure(nameof(request.Body.QRCodeContent),
                            "A 'QRCodeContent' nem lehet üres a megadott terméknél.")
                    });

                try
                {
                    var qrCodeContentDeserialized =
                        JsonSerializer.Deserialize<QRCodeImageContent>(request.Body.QRCodeContent);

                    if (qrCodeContentDeserialized == null)
                        throw new Exception();

                    var qrCode = ownedItem.Product.QRCodes.FirstOrDefault(c =>
                        c.Id == qrCodeContentDeserialized.Id && c.Secret == qrCodeContentDeserialized.Secret);

                    if (qrCode == null)
                        throw new Exception();

                    qrCodeEmail = qrCode.Email;
                }
                catch
                {
                    throw new BadRequestException("Hibás QR kód tartalom.");
                }
            }

            // Validate and transform inputs
            if (ownedItem.Product.Inputs.Count > 0 && request.Body.Inputs == null)
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(request.Body.Inputs),
                        "A 'Inputs' mező nem lehet üres a megadott terméknél.")
                });

            var inputLabelsToValues = new Dictionary<string, string>();
            foreach (var input in ownedItem.Product.Inputs)
            {
                if (!request.Body.Inputs.ContainsKey(input.Key) || string.IsNullOrEmpty(request.Body.Inputs[input.Key]))
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure(nameof(request.Body.Inputs),
                            $"A '{input.Key}' mező nem lehet üres a megadott terméknél.")
                    });

                switch (input.Type)
                {
                    case ProductInputType.Boolean:
                        if (bool.TryParse(request.Body.Inputs[input.Key], out var booleanValue))
                            inputLabelsToValues.Add(input.Label, booleanValue ? "Igen" : "Nem");
                        else
                            throw new ValidationException(new[]
                            {
                                new ValidationFailure(nameof(request.Body.Inputs),
                                    $"A '{input.Key}' mező értéke nem egy érvényes bool (true/false) érték.")
                            });
                        break;
                    case ProductInputType.Textbox:
                        inputLabelsToValues.Add(input.Label, request.Body.Inputs[input.Key]);
                        break;
                }
            }

            await _context.Database.BeginTransactionAsync(cancellationToken);

            ownedItem.UsedAt = DateTime.Now.ToUniversalTime();

            await _context.OwnedItemUses.AddAsync(new OwnedItemUse
            {
                Values = request.Body.Inputs ?? new Dictionary<string, string>(),
                OwnedItem = ownedItem
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            await _context.Database.CommitTransactionAsync(cancellationToken);

            await _publisher.Publish(new OwnedItemUsedEvent
            {
                Product = ownedItem.Product,
                User = _userAccessor.User,
                InputValues = inputLabelsToValues,
                QRCodeEmail = qrCodeEmail
            }, cancellationToken);

            return Unit.Value;
        }
    }
}