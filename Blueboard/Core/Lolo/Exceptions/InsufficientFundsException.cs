using Blueboard.Core.Lolo.Services;

namespace Blueboard.Core.Lolo.Exceptions;

/// <summary>
///     The exception thrown when a user is trying to spend more coins in <see cref="LoloManager" /> than they have.
/// </summary>
public class InsufficientFundsException : InvalidOperationException
{
}