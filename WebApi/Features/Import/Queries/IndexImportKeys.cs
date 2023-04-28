using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Import.Queries;

public static class IndexImportKeys
{
    public class Query : IRequest<IEnumerable<Response>>
    {
    }

    public class Response
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool Enabled { get; set; }
    }

    internal sealed class
        Handler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Response>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var importKeys = await _context.ImportKeys.ToListAsync();

            return importKeys.Adapt<IEnumerable<Response>>();
        }
    }
}