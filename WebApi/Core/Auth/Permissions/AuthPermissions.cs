using WebApi.Core.Auth.Interfaces;

namespace WebApi.Core.Auth.Permissions;

public static class AuthPermissions
{
    public class Control : IPermission
    {
        public string Name { get; } = "Auth.Control";
        public string DisplayName { get; } = "Control";
        public string Description { get; } = "Én ezt nem venném el, megbénítja az adott csoport minden felhasználóját";
    }

    public class IndexPermissions : IPermission
    {
        public string Name { get; } = "Auth.IndexPermissions";
        public string DisplayName { get; } = "Jogosultságok listázása";
        public string Description { get; } = "Az összes jogosultság lekérése és listázása";
    }

    public class IndexUserGroups : IPermission
    {
        public string Name { get; } = "Auth.IndexUserGroups";
        public string DisplayName { get; } = "Felhasználói csoportok listázása";
        public string Description { get; } = "Az összes felhasználói csoport lekérése és listázása";
    }

    public class ViewUserGroup : IPermission
    {
        public string Name { get; } = "Auth.ViewUserGroup";
        public string DisplayName { get; } = "Felhasználói csoport megtekintése";
        public string Description { get; } = "Egy adott felhasználói csoport lekérése és megtekintése id alapján";
    }
}