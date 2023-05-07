using WebApi.Core.Auth.Schemes.Token;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Core.Auth.Jobs;

/// <summary>
///     The background job that updates the last used at property of a personal access token. Fired each time when a token
///     is used in <see cref="TokenAuthenticationSchemeHandler" />.
/// </summary>
public class UpdateTokenLastUsedAtJob
{
    private readonly IServiceProvider _serviceProvider;

    public UpdateTokenLastUsedAtJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Run(int id, DateTime lastUsedAt)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var token = context.PersonalAccessTokens.Find(id);
        if (token == null) return;
        token.LastUsedAt = lastUsedAt;
        context.SaveChanges(); // Can't use async here, because Hangfire doesn't support it
    }
}