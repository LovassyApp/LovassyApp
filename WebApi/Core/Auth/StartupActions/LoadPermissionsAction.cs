using System.Reflection;
using Helpers.Framework.Interfaces;
using WebApi.Core.Auth.Utils;

namespace WebApi.Core.Auth.StartupActions;

public class LoadPermissionsAction : IStartupAction
{
    private readonly ILogger<LoadPermissionsAction> _logger;

    public LoadPermissionsAction(ILogger<LoadPermissionsAction> logger)
    {
        _logger = logger;
    }

    public async Task Execute()
    {
        // Basically, the reason this works is because startup actions are executed right after app.Build()
        // Now when we do app.MapControllers() in the startup, it will require the permissions to be loaded
        // So we always have to pay attention to this, but if we don't it's luckily an error right at startup
        PermissionUtils.LoadPermissions(Assembly.GetExecutingAssembly());
        _logger.LogInformation("Loaded permissions into PermissionUtils");
    }
}