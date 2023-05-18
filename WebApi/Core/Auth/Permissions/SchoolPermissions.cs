using WebApi.Core.Auth.Interfaces;

namespace WebApi.Core.Auth.Permissions;

public static class SchoolPermissions
{
    public class ViewGrades : IPermission
    {
        public string Name { get; } = "School.ViewGrades";
        public string DisplayName { get; } = "Jegyek megtekintése";
        public string Description { get; } = "Saját jegyek megtekintése";
    }
}