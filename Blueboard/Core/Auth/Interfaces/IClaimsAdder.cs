using System.Security.Claims;

namespace Blueboard.Core.Auth.Interfaces;

/// <summary>
///     The interface for adding claims to a <see cref="ClaimsPrincipal" />. Used in authentication schemes. They have to
///     be registered manually.
/// </summary>
/// <typeparam name="TUser"></typeparam>
public interface IClaimsAdder<in TUser>
{
    /// <summary>
    ///     Adds claims to the <see cref="ClaimsPrincipal" />.
    /// </summary>
    /// <param name="claims">
    ///     The list of claims that will be added to the <see creaf="ClaimsPrincipal" />. It should be
    ///     mutated.
    /// </param>
    /// <param name="user">The "user model", from which to get the claims data.</param>
    /// <returns></returns>
    public Task AddClaimsAsync(List<Claim> claims, TUser user);
}