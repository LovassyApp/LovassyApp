using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Import.Queries;

public static class IndexImportKeys
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
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
        private readonly SieveProcessor _sieveProcessor;

        public Handler(ApplicationDbContext context, SieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<IEnumerable<Response>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var importKeys = _context.ImportKeys;

            var filteredImportKeys =
                await _sieveProcessor.Apply(request.SieveModel, importKeys).ToListAsync(cancellationToken);

            return filteredImportKeys.Adapt<IEnumerable<Response>>();
        }
    }
}