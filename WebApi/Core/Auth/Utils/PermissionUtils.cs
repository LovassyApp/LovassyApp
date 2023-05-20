using System.Reflection;
using System.Security.Claims;
using WebApi.Core.Auth.Interfaces;

namespace WebApi.Core.Auth.Utils;

/// <summary>
///     Utility class for working with permissions. It's a bit unusual for a utility class to contain static data members,
///     but we kinda need it to have a nice developer experience.
/// </summary>
public static class PermissionUtils
{
    //TODO: Explore the possibility of using a source generator for this
    public static List<IPermission>? Permissions { get; private set; }
    public static Dictionary<Type, string>? PermissionTypesToNames { get; private set; }

    /// <summary>
    ///     Loads all permissions from the given assembly into the static list of permission objects and static dictionary
    ///     mapping types to names. Has to be called before <c>app.MapControllers()</c>.
    /// </summary>
    /// <param name="assembly">The assembly containing all permissions.</param>
    public static void LoadPermissions(Assembly assembly)
    {
        var permissionTypes = assembly.GetTypes()
            .Where(x => typeof(IPermission).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();

        Permissions = permissionTypes.Select(x =>
            (IPermission)Activator.CreateInstance(x)!
        ).ToList();

        PermissionTypesToNames = Permissions.ToDictionary(x => x.GetType(), x => x.Name);
    }

    /// <summary>
    ///     Checks if the given <see cref="ClaimsPrincipal" /> has at least one of the given permissions. Will always return
    ///     true if the user is a super user.
    /// </summary>
    /// <param name="claimsPrincipal">
    ///     The <see cref="ClaimsPrincipal" /> containing the <see cref="AuthConstants.PermissionClaim" />
    ///     claims.
    /// </param>
    /// <param name="permissions">
    ///     The array of permissions to check for. They are supposed to correspond to permission names
    ///     defined in any of the derived types of <see cref="IPermission" />.
    /// </param>
    /// <returns>Whether the given claims principal has at least one of the given permissions.</returns>
    public static bool CheckPermissions(ClaimsPrincipal claimsPrincipal, string[] permissions)
    {
        if (permissions.Length == 0 || CheckSuperUser(claimsPrincipal))
            return true;

        var userPermissions = claimsPrincipal.FindAll(AuthConstants.PermissionClaim).ToArray();

        if (userPermissions.Length == 0)
            return false;

        var userPermissionsSet = new HashSet<string>(userPermissions.Select(p => p.Value));

        foreach (var permission in permissions)
            if (userPermissionsSet.Contains(permission))
                return true;

        return false;
    }

    /// <summary>
    ///     Checks if the given <see cref="ClaimsPrincipal" /> is a super user. (Has permission to do anything)
    /// </summary>
    /// <param name="claimsPrincipal">
    ///     The <see cref="ClaimsPrincipal" /> containing the
    ///     <see cref="AuthConstants.SuperUserClaim" /> claim.
    /// </param>
    /// <returns>Whether the user is a super user or not.</returns>
    public static bool CheckSuperUser(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(AuthConstants.SuperUserClaim) == bool.TrueString;
    }
}