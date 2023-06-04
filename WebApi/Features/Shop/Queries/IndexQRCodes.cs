using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Shop.Queries;

public static class IndexQRCodes
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly ApplicationDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public Handler(ApplicationDbContext context, SieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var qrcodes = _context.QRCodes.AsNoTracking();

            var filteredQrcodes =
                await _sieveProcessor.Apply(request.SieveModel, qrcodes).ToListAsync(cancellationToken);

            return filteredQrcodes.Adapt<IEnumerable<Response>>();
        }
    }
}