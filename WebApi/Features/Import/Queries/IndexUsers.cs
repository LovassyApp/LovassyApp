using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Import.Queries;

public class IndexUsersQuery : IRequest<IEnumerable<IndexUsersResponse>>
{
}

public class IndexUsersResponse
{
    public Guid Id { get; set; }
    public string OmCodeHashed { get; set; }
    public string PublicKey { get; set; }
}

internal sealed class IndexUsersQueryHandler : IRequestHandler<IndexUsersQuery, IEnumerable<IndexUsersResponse>>
{
    private readonly ApplicationDbContext _context;

    public IndexUsersQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<IndexUsersResponse>> Handle(IndexUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync();

        return users.Adapt<IEnumerable<IndexUsersResponse>>();
    }
}