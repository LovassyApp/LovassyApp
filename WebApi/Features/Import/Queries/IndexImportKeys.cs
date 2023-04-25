using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Import.Queries;

public class IndexImportKeysQuery : IRequest<IEnumerable<IndexImportKeyResponse>>
{
}

public class IndexImportKeyResponse
{
    public int Id { get; set; }

    public string Name { get; set; }
    public bool Enabled { get; set; }
}

internal sealed class
    IndexImportKeysQueryHandler : IRequestHandler<IndexImportKeysQuery, IEnumerable<IndexImportKeyResponse>>
{
    private readonly ApplicationDbContext _context;

    public IndexImportKeysQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<IndexImportKeyResponse>> Handle(IndexImportKeysQuery request,
        CancellationToken cancellationToken)
    {
        var importKeys = await _context.ImportKeys.ToListAsync();

        return importKeys.Adapt<IEnumerable<IndexImportKeyResponse>>();
    }
}