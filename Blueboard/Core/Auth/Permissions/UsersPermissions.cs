using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class UsersPermissions
{
    public class IndexUsers : IPermission
    {
        public string Name => "Users.IndexUsers";
        public string DisplayName => "Felhasználók listázása";
        public string Description => "Az összes felhasználó lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewUser : IPermission
    {
        public string Name => "Users.ViewUser";
        public string DisplayName => "Felhasználó megtekintése";
        public string Description => "Egy adott felhasználó lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class UpdateUser : IPermission
    {
        public string Name => "Users.UpdateUser";
        public string DisplayName => "Felhasználó módosítása";
        public string Description => "Egy adott felhasználó módosítása id alapján";
        public bool Dangerous => true;
    }

    public class DeleteUser : IPermission
    {
        public string Name => "Users.DeleteUser";
        public string DisplayName => "Felhasználó törlése";
        public string Description => "Egy adott felhasználó törlése id alapján";
        public bool Dangerous => true;
    }

    public class KickUser : IPermission
    {
        public string Name => "Users.KickUser";
        public string DisplayName => "Felhasználó kirúgása";
        public string Description => "Egy adott felhasználó kirúgása (kijelentkeztetése) id alapján";
        public bool Dangerous => true;
    }

    public class KickAllUsers : IPermission
    {
        public string Name => "Users.KickAllUsers";
        public string DisplayName => "Összes felhasználó kirúgása";
        public string Description => "Az összes felhasználó kirúgása (kijelentkeztetése)";
        public bool Dangerous => true;
    }
}