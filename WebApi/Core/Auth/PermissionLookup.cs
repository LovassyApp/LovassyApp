using System.Linq.Expressions;
using System.Reflection;
using WebApi.Core.Auth.Interfaces;

namespace WebApi.Core.Auth;

/// <summary>
///     The class used for loading permissions and looking up permissions by name or type
/// </summary>
public static class PermissionLookup
{
    public static ILookup<string, Type>? NamesToPermissions { get; private set; }
    public static ILookup<Type, string>? PermissionsToNames { get; private set; }

    /// <summary>
    ///     Loads all <see cref="IPermission" /> implementations from the given <see cref="Assembly" />. This method should be
    ///     called on startup before the first request.
    /// </summary>
    /// <param name="assembly">The assembly containing the permission types.</param>
    public static void LoadPermissions(Assembly assembly)
    {
        var permissions = assembly.GetTypes()
            .Where(x => typeof(IPermission).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();

        NamesToPermissions = permissions.ToLookup(x =>
        {
            // This is the fastest way to create an instance of a type
            var newExpression = Expression.New(x);
            var lambda = Expression.Lambda<Func<object>>(newExpression);
            var func = lambda.Compile();
            return ((IPermission)func()).Name;
        }, x => x);

        PermissionsToNames = permissions.ToLookup(x => x, x =>
        {
            var newExpression = Expression.New(x);
            var lambda = Expression.Lambda<Func<object>>(newExpression);
            var func = lambda.Compile();
            return ((IPermission)func()).Name;
        });
    }
}