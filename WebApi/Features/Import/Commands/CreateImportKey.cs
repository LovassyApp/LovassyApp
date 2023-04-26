using System.Security.Cryptography;
using FluentValidation;
using Mapster;
using MediatR;
using WebApi.Core.Cryptography.Services;
using WebApi.Features.Import.Queries;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Import.Commands;

public class CreateImportKeyCommand : IRequest<ViewImportKeyResponse>
{
    public CreateImportKeyBody Body { get; set; }
}

public class CreateImportKeyBody
{
    public string Name { get; set; }
    public bool Enabled { get; set; }
}

public class CreateImportKeyBodyValidator : AbstractValidator<CreateImportKeyBody>
{
    public CreateImportKeyBodyValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Enabled).NotNull();
    }
}

internal sealed class CreateImportKeyCommandHandler : IRequestHandler<CreateImportKeyCommand, ViewImportKeyResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly EncryptionService _encryptionService;
    private readonly HashService _hashService;

    public CreateImportKeyCommandHandler(ApplicationDbContext context, EncryptionService encryptionService,
        HashService hashService)
    {
        _context = context;
        _encryptionService = encryptionService;
        _hashService = hashService;
    }

    public async Task<ViewImportKeyResponse> Handle(CreateImportKeyCommand request, CancellationToken cancellationToken)
    {
        var importKey = request.Body.Adapt<ImportKey>();
        var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(512 / 8));

        importKey.KeyProtected = _encryptionService.Protect(key);
        importKey.KeyHashed = _hashService.Hash(key);

        await _context.ImportKeys.AddAsync(importKey, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = importKey.Adapt<ViewImportKeyResponse>();
        response.Key = key;

        return response;
    }
}