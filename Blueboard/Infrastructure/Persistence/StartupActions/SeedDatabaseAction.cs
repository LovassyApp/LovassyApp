using System.Reflection;
using Blueboard.Core.Auth;
using Blueboard.Core.Auth.Interfaces;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Infrastructure.Persistence.StartupActions;

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
        var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        var permissionTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => typeof(IPermission).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
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
                Permissions = environment.IsDevelopment() ? permissionNames : new string[] { }
            };

            await context.UserGroups.AddAsync(group);
            await context.SaveChangesAsync();

            await context.Database.ExecuteSqlRawAsync(
                @$"select setval(pg_get_serial_sequence('""{nameof(ApplicationDbContext.UserGroups)}""', '{nameof(UserGroup.Id)}'), (select max(""{nameof(UserGroup.Id)}"") from ""{nameof(ApplicationDbContext.UserGroups)}""))");
        }
    }
}