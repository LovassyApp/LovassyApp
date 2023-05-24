using WebApi.Core.Auth.Interfaces;

namespace WebApi.Core.Auth.Permissions;

public static class UsersPermissions
{
    public class IndexUsers : IPermission
    {
        public string Name { get; } = "Users.IndexUsers";
        public string DisplayName { get; } = "Felhasználók listázása";
        public string Description { get; } = "Az összes felhasználó lekérése és listázása";
    }

    public class ViewUser : IPermission
    {
        public string Name { get; } = "Users.ViewUser";
        public string DisplayName { get; } = "Felhasználó megtekintése";
        public string Description { get; } = "Egy adott felhasználó lekérése és megtekintése id alapján";
    }
}