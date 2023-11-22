using Blueboard.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Import.Queries;

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

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal sealed class
        Handler(ApplicationDbContext context, SieveProcessor sieveProcessor)
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var importKeys = context.ImportKeys;

            var filteredImportKeys =
                await sieveProcessor.Apply(request.SieveModel, importKeys).ToListAsync(cancellationToken);

            return filteredImportKeys.Adapt<IEnumerable<Response>>();
        }
    }
}