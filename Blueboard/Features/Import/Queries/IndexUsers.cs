using Blueboard.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Blueboard.Features.Import.Queries;

public static class IndexUsers
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public SieveModel SieveModel { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string OmCodeHashed { get; set; }
        public string PublicKey { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext context, SieveProcessor sieveProcessor)
        : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var users = context.Users.Where(u => u.EmailVerifiedAt != null).AsNoTracking();

            var filteredUsers = await sieveProcessor.Apply(request.SieveModel, users).ToListAsync(cancellationToken);

            return filteredUsers.Adapt<IEnumerable<Response>>();
        }
    }
}