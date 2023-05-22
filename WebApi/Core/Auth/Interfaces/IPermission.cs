using Sieve.Attributes;

namespace WebApi.Core.Auth.Interfaces;

public interface IPermission
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string DisplayName { get; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string Description { get; }
}