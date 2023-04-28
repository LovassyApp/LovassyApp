using System.Security.Cryptography;
using FluentValidation;
using Mapster;
using MediatR;
using WebApi.Core.Cryptography.Services;
using WebApi.Features.Import.Queries;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Import.Commands;

public static class CreateImportKey
{
    public class Command : IRequest<ViewImportKey.Response>
    {
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Enabled).NotNull();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, ViewImportKey.Response>
    {
        private readonly ApplicationDbContext _context;
        private readonly EncryptionService _encryptionService;
        private readonly HashService _hashService;

        public Handler(ApplicationDbContext context, EncryptionService encryptionService,
            HashService hashService)
        {
            _context = context;
            _encryptionService = encryptionService;
            _hashService = hashService;
        }

        public async Task<ViewImportKey.Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var importKey = request.Body.Adapt<ImportKey>();
            var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(512 / 8));

            importKey.KeyProtected = _encryptionService.Protect(key);
            importKey.KeyHashed = _hashService.Hash(key);

            await _context.ImportKeys.AddAsync(importKey, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = importKey.Adapt<ViewImportKey.Response>();
            response.Key = key;

            return response;
        }
    }
}