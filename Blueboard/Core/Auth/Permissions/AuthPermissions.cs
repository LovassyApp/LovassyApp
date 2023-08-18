using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class AuthPermissions
{
    public class ViewControl : IPermission
    {
        public string Name => "Auth.Control";
        public string DisplayName => "Control lekérése";
        public string Description => "Én ezt nem venném el, megbénítja az adott csoport minden felhasználóját";
        public bool Dangerous => false;
    }

    public class IndexPermissions : IPermission
    {
        public string Name => "Auth.IndexPermissions";
        public string DisplayName => "Jogosultságok listázása";
        public string Description => "Az összes jogosultság lekérése és listázása";
        public bool Dangerous => false;
    }

    public class IndexUserGroups : IPermission
    {
        public string Name => "Auth.IndexUserGroups";
        public string DisplayName => "Felhasználói csoportok listázása";
        public string Description => "Az összes felhasználói csoport lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewUserGroup : IPermission
    {
        public string Name => "Auth.ViewUserGroup";
        public string DisplayName => "Felhasználói csoport megtekintése";
        public string Description => "Egy adott felhasználói csoport lekérése és megtekintése id alapján";
        public bool Dangerous => false;
    }

    public class CreateUserGroup : IPermission
    {
        public string Name => "Auth.CreateUserGroup";
        public string DisplayName => "Felhasználói csoport létrehozása";
        public string Description => "Új felhasználói csoport létrehozása";
        public bool Dangerous => false;
    }

    public class UpdateUserGroup : IPermission
    {
        public string Name => "Auth.UpdateUserGroup";
        public string DisplayName => "Felhasználói csoport módosítása";
        public string Description => "Egy adott felhasználói csoport módosítása id alapján";
        public bool Dangerous => true;
    }

    public class DeleteUserGroup : IPermission
    {
        public string Name => "Auth.DeleteUserGroup";
        public string DisplayName => "Felhasználói csoport törlése";
        public string Description => "Egy adott felhasználói csoport törlése id alapján";
        public bool Dangerous => true;
    }
}