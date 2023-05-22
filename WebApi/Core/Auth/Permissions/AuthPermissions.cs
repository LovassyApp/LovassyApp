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
}