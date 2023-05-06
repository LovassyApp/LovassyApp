using WebApi.Infrastructure.Persistence;

namespace WebApi.Core.Auth.Jobs;

public class DeleteOldTokensJob
{
    private readonly ILogger<DeleteOldTokensJob> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DeleteOldTokensJob(IServiceProvider serviceProvider, ILogger<DeleteOldTokensJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void Run()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var tokens =
            context.PersonalAccessTokens.Where(x =>
                x.LastUsedAt != null && x.LastUsedAt < DateTime.Now.AddDays(-7).ToUniversalTime());
        var count = tokens.Count();
        context.PersonalAccessTokens.RemoveRange(tokens);
        context.SaveChanges();
        _logger.LogInformation("Deleted {Count} old tokens", count);
    }
}