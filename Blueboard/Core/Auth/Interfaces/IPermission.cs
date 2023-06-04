using Blueboard.Core.Auth.Utils;
using Sieve.Attributes;

namespace Blueboard.Core.Auth.Interfaces;

/// <summary>
///     The interface representing a permission. Permissions with this interface should be automatically discovered and
///     registered in <see cref="PermissionUtils" /> on startup.
/// </summary>
public interface IPermission
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string DisplayName { get; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string Description { get; }
}