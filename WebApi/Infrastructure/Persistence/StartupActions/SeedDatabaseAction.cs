using System.Reflection;
using Helpers.Framework.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        using var scope = _serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var permissionTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(IPermission).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();

        var permissionNames = permissionTypes.Select(x =>
            ((IPermission)Activator.CreateInstance(x)!).Name
        ).ToArray(); // We can't use PermissionUtils just yet as it is also initialized in a startup action

        var currentDefaultGroup = await context.UserGroups.FindAsync(AuthConstants.DefaultUserGroupID);

        if (currentDefaultGroup != null && !currentDefaultGroup.Permissions.SequenceEqual(permissionNames))
        {
            currentDefaultGroup.Permissions = permissionNames;
            await context.SaveChangesAsync();
        }
        else if (currentDefaultGroup == null)
        {
            var group = new UserGroup
            {
                Id = AuthConstants.DefaultUserGroupID,
                Name = "Default",
                // TODO: Add a check for the environment and act accordingly
                Permissions = permissionNames
            };

            await context.UserGroups.AddAsync(group);
            await context.SaveChangesAsync();

            await context.Database.ExecuteSqlRawAsync(
                @$"select setval(pg_get_serial_sequence('""{nameof(ApplicationDbContext.UserGroups)}""', '{nameof(UserGroup.Id)}'), (select max(""{nameof(UserGroup.Id)}"") from ""{nameof(ApplicationDbContext.UserGroups)}""))");
        }
    }
}