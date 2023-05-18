using WebApi.Core.Auth.Interfaces;

namespace WebApi.Core.Auth.Permissions;

public class ShopPermissions
{
    public class ViewLolo : IPermission
    {
        public string Name { get; } = "Shop.ViewLolo";
        public string DisplayName { get; } = "Loló megtekintése";
        public string Description { get; } = "Loló megtekintése";
    }
}