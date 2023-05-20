using System.Reflection;
using Helpers.Framework.Interfaces;
using WebApi.Core.Auth;
using WebApi.Core.Auth.Interfaces;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Infrastructure.Persistence.StartupActions;

public class SeedDatabaseAction : IStartupAction
{
    private readonly IServiceProvider _serviceProvider;

    public SeedDatabaseAction(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Execute()
    {
        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await context.UserGroups.FindAsync(AuthConstants.DefaultUserGroupID) != null)
            return;

        var permissionTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(IPermission).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();

        var permissionNames = permissionTypes.Select(x =>
            ((IPermission)Activator.CreateInstance(x)!).Name
        ).ToArray();

        var group = new UserGroup
        {
            Id = AuthConstants.DefaultUserGroupID,
            Name = "Default",
            //TODO: Add a check for the environment and act accordingly
            Permissions =
                permissionNames // We can't use PermissionLookup just yet as it is also initialized in a startup action
        };

        await context.UserGroups.AddAsync(group);
        await context.SaveChangesAsync();
    }
}