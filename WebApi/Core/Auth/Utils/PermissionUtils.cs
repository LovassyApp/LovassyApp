using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using WebApi.Core.Auth.Interfaces;

namespace WebApi.Core.Auth.Utils;

public static class PermissionUtils
{
    public static List<IPermission>? Permissions { get; private set; }
    public static Dictionary<Type, string>? PermissionTypesToNames { get; private set; }

    public static void LoadPermissions(Assembly assembly)
    {
        var permissionTypes = assembly.GetTypes()
            .Where(x => typeof(IPermission).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();

        Permissions = permissionTypes.Select(x =>
        {
            // This is the fastest way to create an instance of a type
            var newExpression = Expression.New(x);
            var lambda = Expression.Lambda<Func<IPermission>>(newExpression);
            var func = lambda.Compile();
            return func();
        }).ToList();

        PermissionTypesToNames = Permissions.ToDictionary(x => x.GetType(), x => x.Name);
    }

    public static bool CheckPermissions(ClaimsPrincipal user, string[] permissions)
    {
        if (permissions.Length == 0)
            return true;

        var userPermissions = user.FindAll(AuthConstants.PermissionClaim).ToArray();

        if (userPermissions?.Length == 0)
            return false;

        var userPermissionsSet = new HashSet<string>(userPermissions.Select(p => p.Value));

        foreach (var permission in permissions)
            if (userPermissionsSet.Contains(permission))
                return true;

        return false;
    }
}