using Blueboard.Features.Users.Jobs;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shimmer.Services;
using SessionOptions = Blueboard.Core.Auth.Services.Options.SessionOptions;

namespace Blueboard.Features.Users.Commands;

public static class KickUser
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler(
        ApplicationDbContext context,
        IShimmerJobFactory jobFactory,
        IOptions<SessionOptions> sessionOptions)
        : IRequestHandler<Command>
    {
        private readonly SessionOptions _sessionOptions = sessionOptions.Value;

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users.Where(u => u.Id == request.Id).Include(u =>
                    u.PersonalAccessTokens.Where(t =>
                        t.CreatedAt >= DateTime.UtcNow.AddMinutes(-_sessionOptions.ExpiryMinutes)))
                .AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            if (user == null) throw new NotFoundException(nameof(User), request.Id);

            var kickUsersJob = await jobFactory.CreateAsync<KickUsersJob, KickUsersJob.Data>(cancellationToken);

            kickUsersJob.Data(new KickUsersJob.Data
            {
                Tokens = user.PersonalAccessTokens.Select(t => t.Token).ToList()
            });

            await kickUsersJob.FireAsync(cancellationToken);

            return Unit.Value;
        }
    }
}