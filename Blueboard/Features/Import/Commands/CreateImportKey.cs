using System.Security.Cryptography;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentValidation;
using Helpers.Cryptography.Services;
using Mapster;
using MediatR;

namespace Blueboard.Features.Import.Commands;

public static class CreateImportKey
{
    public class Command : IRequest<Response>
    {
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool Enabled { get; set; }

        public string Key { get; set; }
    }

    public class RequestBodyValidator : AbstractValidator<RequestBody>
    {
        public RequestBodyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Enabled).NotNull();
        }
    }

    internal sealed class Handler(ApplicationDbContext context, EncryptionService encryptionService,
            HashService hashService)
        : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var importKey = request.Body.Adapt<ImportKey>();
            var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(512 / 8));

            importKey.KeyProtected = encryptionService.Protect(key);
            importKey.KeyHashed = hashService.Hash(key);

            await context.ImportKeys.AddAsync(importKey, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var response = importKey.Adapt<Response>();
            response.Key = key;

            return response;
        }
    }
}