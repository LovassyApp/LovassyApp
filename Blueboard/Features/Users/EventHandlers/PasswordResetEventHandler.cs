using Blueboard.Features.Auth.Events;
using Blueboard.Features.Users.Jobs;
using Blueboard.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shimmer.Services;
using SessionOptions = Blueboard.Core.Auth.Services.Options.SessionOptions;

namespace Blueboard.Features.Users.EventHandlers;

public class PasswordResetEventHandler(
    ApplicationDbContext context,
    IShimmerJobFactory jobFactory,
    IOptions<SessionOptions> sessionOptions)
    : INotificationHandler<PasswordResetEvent>
{
    private readonly SessionOptions _sessionOptions = sessionOptions.Value;


    public async Task Handle(PasswordResetEvent notification, CancellationToken cancellationToken)
    {
        // We want to kick the user when they reset their password, because if their password was compromised, we want to make sure that the attacker can't use the old token to access the account
        var personalAccessTokens = await context.PersonalAccessTokens.Where(t => t.UserId == notification.User.Id)
            .Where(t => t.CreatedAt >= DateTime.UtcNow.AddMinutes(-_sessionOptions.ExpiryMinutes)).AsNoTracking()
            .ToListAsync(cancellationToken);

        var kickUsersJob = await jobFactory.CreateAsync<KickUsersJob, KickUsersJob.Data>(cancellationToken);

        kickUsersJob.Data(new KickUsersJob.Data
        {
            Tokens = personalAccessTokens.Select(t => t.Token).ToList()
        });

        await kickUsersJob.FireAsync(cancellationToken);
    }
}