using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class UsersPermissions
{
    public class IndexUsers : IPermission
    {
        public string Name { get; } = "Users.IndexUsers";
        public string DisplayName { get; } = "Felhasználók listázása";
        public string Description { get; } = "Az összes felhasználó lekérése és listázása";
        public bool Dangerous { get; } = false;
    }

    public class ViewUser : IPermission
    {
        public string Name { get; } = "Users.ViewUser";
        public string DisplayName { get; } = "Felhasználó megtekintése";
        public string Description { get; } = "Egy adott felhasználó lekérése és megtekintése id alapján";
        public bool Dangerous { get; } = false;
    }

    public class UpdateUser : IPermission
    {
        public string Name { get; } = "Users.UpdateUser";
        public string DisplayName { get; } = "Felhasználó módosítása";
        public string Description { get; } = "Egy adott felhasználó módosítása id alapján";
        public bool Dangerous { get; } = true;
    }

    public class DeleteUser : IPermission
    {
        public string Name { get; } = "Users.DeleteUser";
        public string DisplayName { get; } = "Felhasználó törlése";
        public string Description { get; } = "Egy adott felhasználó törlése id alapján";
        public bool Dangerous { get; } = true;
    }

    public class KickUser : IPermission
    {
        public string Name { get; } = "Users.KickUser";
        public string DisplayName { get; } = "Felhasználó kirúgása";
        public string Description { get; } = "Egy adott felhasználó kirúgása (kijelentkeztetése) id alapján";
        public bool Dangerous { get; } = true;
    }

    public class KickAllUsers : IPermission
    {
        public string Name { get; } = "Users.KickAllUsers";
        public string DisplayName { get; } = "Összes felhasználó kirúgása";
        public string Description { get; } = "Az összes felhasználó kirúgása (kijelentkeztetése)";
        public bool Dangerous { get; } = true;
    }
}