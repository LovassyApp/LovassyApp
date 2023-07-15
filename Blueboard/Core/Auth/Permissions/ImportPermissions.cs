using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class ImportPermissions
{
    public class IndexImportKeys : IPermission
    {
        public string Name => "Import.IndexImportKeys";
        public string DisplayName => "Import kulcsok listázása";
        public string Description => "Az összes import kulcs lekérése és listázása";
        public bool Dangerous => false;
    }

    public class ViewImportKey : IPermission
    {
        public string Name => "Import.ViewImportKey";
        public string DisplayName => "Import kulcs megtekintése";
        public string Description => "Egy adott import kulcs lekérése és megtekintése id alapján";
        public bool Dangerous => true;
    }

    public class CreateImportKey : IPermission
    {
        public string Name => "Import.CreateImportKey";
        public string DisplayName => "Import kulcs létrehozása";
        public string Description => "Új import kulcs létrehozása";
        public bool Dangerous => true;
    }

    public class UpdateImportKey : IPermission
    {
        public string Name => "Import.UpdateImportKey";
        public string DisplayName => "Import kulcs módosítása";
        public string Description => "Egy adott import kulcs módosítása id alapján";
        public bool Dangerous => true;
    }

    public class DeleteImportKey : IPermission
    {
        public string Name => "Import.DeleteImportKey";
        public string DisplayName => "Import kulcs törlése";
        public string Description => "Egy adott import kulcs törlése id alapján";
        public bool Dangerous => true;
    }
}