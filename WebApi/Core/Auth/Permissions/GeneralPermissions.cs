using WebApi.Core.Auth.Interfaces;

namespace WebApi.Core.Auth.Permissions;

public static class GeneralPermissions
{
    public class Control : IPermission
    {
        public string Name { get; } = "General.Control";
        public string DisplayName { get; } = "Control";
        public string Description { get; } = "Én ezt nem venném el, megbénítja az adott csoport minden felhasználóját";
    }
}