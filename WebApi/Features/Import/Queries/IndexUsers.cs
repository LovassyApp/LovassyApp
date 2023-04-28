using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Features.Import.Queries;

public static class IndexUsers
{
    public class Query : IRequest<IEnumerable<Response>>
    {
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string OmCodeHashed { get; set; }
        public string PublicKey { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Response>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var users = await _context.Users.ToListAsync();

            return users.Adapt<IEnumerable<Response>>();
        }
    }
}