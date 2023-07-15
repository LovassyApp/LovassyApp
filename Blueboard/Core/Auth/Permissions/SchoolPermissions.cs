using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class SchoolPermissions
{
    public class IndexGrades : IPermission
    {
        public string Name => "School.IndexGrades";
        public string DisplayName => "Jegyek lekérése";
        public string Description => "Saját jegyek lekérése és listázása";
        public bool Dangerous => false;
    }
}