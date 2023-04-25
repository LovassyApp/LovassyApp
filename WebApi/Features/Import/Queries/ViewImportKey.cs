using Mapster;
using MediatR;
using WebApi.Common.Exceptions;
using WebApi.Core.Cryptography.Services;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Import.Queries;

public class ViewImportKeyQuery : IRequest<ViewImportKeyResponse>
{
    public int Id { get; set; }
}

public class ViewImportKeyResponse
{
    public int Id { get; set; }

    public string Name { get; set; }
    public bool Enabled { get; set; }

    public string Key { get; set; }
}

internal sealed class ViewImportKeyQueryHandler : IRequestHandler<ViewImportKeyQuery, ViewImportKeyResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly EncryptionService _encryptionService;

    public ViewImportKeyQueryHandler(ApplicationDbContext context, EncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    public async Task<ViewImportKeyResponse> Handle(ViewImportKeyQuery request, CancellationToken cancellationToken)
    {
        var importKey = await _context.ImportKeys.FindAsync(request.Id);

        if (importKey is null)
            throw new NotFoundException(nameof(ImportKey), request.Id);

        var key = _encryptionService.Unprotect(importKey.KeyProtected);

        var response = importKey.Adapt<ViewImportKeyResponse>();
        response.Key = key;

        return response;
    }
}