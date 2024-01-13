using Blueboard.Features.Users.Jobs;
using Blueboard.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shimmer.Services;
using SessionOptions = Blueboard.Core.Auth.Services.Options.SessionOptions;

namespace Blueboard.Features.Users.Commands;

public static class KickAllUsers
{
    public class Command : IRequest
    {
    }

    internal sealed class Handler(
        ApplicationDbContext context,
        IShimmerJobFactory jobFactory,
        IOptions<SessionOptions> sessionOptions)
        : IRequestHandler<Command>
    {
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var personalAccessTokens = await context.PersonalAccessTokens
                .Where(t => t.CreatedAt >= DateTime.UtcNow.AddMinutes(-sessionOptions.Value.ExpiryMinutes))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var kickUsersJob = await jobFactory.CreateAsync<KickUsersJob, KickUsersJob.Data>(cancellationToken);

            kickUsersJob.Data(new KickUsersJob.Data
            {
                Tokens = personalAccessTokens.Select(t => t.Token).ToList()
            });

            await kickUsersJob.FireAsync(cancellationToken);

            return Unit.Value;
        }
    }
}