using WebApi.Persistence;

namespace WebApi.Core.Auth.Jobs;

public class UpdateTokenLastUsedAtJob
{
    private readonly IServiceProvider _serviceProvider;

    public UpdateTokenLastUsedAtJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Run(int id)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var token = context.PersonalAccessTokens.Find(id);
        if (token == null) return;
        token.LastUsedAt = DateTime.Now;
        context.SaveChanges(); // Can't use async here, because Hangfire doesn't support it
    }
}