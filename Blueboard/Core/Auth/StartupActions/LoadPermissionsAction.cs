using System.Reflection;
using Blueboard.Core.Auth.Utils;
using Helpers.WebApi.Interfaces;

namespace Blueboard.Core.Auth.StartupActions;

public class LoadPermissionsAction(ILogger<LoadPermissionsAction> logger) : IStartupAction
{
    public async Task Execute()
    {
        // Basically, the reason this works is because startup actions are executed right after app.Build()
        // Now when we do app.MapControllers() in the startup, it will require the permissions to be loaded
        // So we always have to pay attention to this, but if we don't it's luckily an error right at startup
        PermissionUtils.LoadPermissions(Assembly.GetExecutingAssembly());
        logger.LogInformation("Loaded permissions into PermissionUtils");
    }
}