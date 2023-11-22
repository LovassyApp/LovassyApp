using System.Reflection;
using Blueboard.Core.Auth;
using Blueboard.Core.Auth.Interfaces;
using Blueboard.Infrastructure.Persistence.Entities;
using Helpers.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Infrastructure.Persistence.StartupActions;

public class SeedDatabaseAction(IServiceProvider serviceProvider, ILogger<SeedDatabaseAction> logger)
    : IStartupAction
{
    public async Task Execute()
    {
        using var scope = serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        var defaultGroup = await context.UserGroups.FindAsync(AuthConstants.DefaultUserGroupID);

        if (defaultGroup == null)
        {
            var permissionNames = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof(IPermission).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
                .Select(x => ((IPermission)Activator.CreateInstance(x)!).Name)
                .ToArray(); // We can't use PermissionUtils just yet as it is also initialized in a startup action

            var group = new UserGroup
            {
                Id = AuthConstants.DefaultUserGroupID,
                Name = "Default",
                Permissions = environment.IsDevelopment() ? permissionNames : new string[] { }
            };

            await context.UserGroups.AddAsync(group);
            await context.SaveChangesAsync();

            await context.Database.ExecuteSqlRawAsync(
                @$"select setval(pg_get_serial_sequence('""{context.Model.FindEntityType(typeof(UserGroup))!.GetTableName()}""', '{nameof(UserGroup.Id)}'), (select max(""{nameof(UserGroup.Id)}"") from ""{context.Model.FindEntityType(typeof(UserGroup))!.GetTableName()}""))");

            logger.LogInformation("Created the default user group");
        }
    }
}