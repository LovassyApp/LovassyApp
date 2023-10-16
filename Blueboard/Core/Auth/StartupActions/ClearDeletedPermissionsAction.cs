using System.Reflection;
using Blueboard.Core.Auth.Interfaces;
using Blueboard.Infrastructure.Persistence;
using Helpers.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Core.Auth.StartupActions;

public class ClearDeletedPermissionsAction : IStartupAction
{
    private readonly ILogger<ClearDeletedPermissionsAction> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ClearDeletedPermissionsAction(IServiceProvider serviceProvider,
        ILogger<ClearDeletedPermissionsAction> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Execute()
    {
        using var scope = _serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var permissionNames = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => typeof(IPermission).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
            .Select(x => ((IPermission)Activator.CreateInstance(x)!).Name)
            .ToHashSet(); // We can't use PermissionUtils just yet as it is also initialized in a startup action

        var groups = await context.UserGroups.ToListAsync();

        foreach (var group in groups)
            group.Permissions = group.Permissions
                .Where(permission => permissionNames.Contains(permission))
                .ToArray();

        await context.SaveChangesAsync();

        _logger.LogInformation("Cleared deleted permissions from user groups");
    }
}