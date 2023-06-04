using Blueboard.Core.Auth.Interfaces;

namespace Blueboard.Core.Auth.Permissions;

public static class SchoolPermissions
{
    public class IndexGrades : IPermission
    {
        public string Name { get; } = "School.IndexGrades";
        public string DisplayName { get; } = "Jegyek lekérése";
        public string Description { get; } = "Saját jegyek lekérése és listázása";
    }
}