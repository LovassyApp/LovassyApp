using WebApi.Core.Backboard.Services;

namespace WebApi.Core.Backboard.Exceptions;

/// <summary>
///     The exception thrown when the user is not found in <see cref="BackboardAdapter" /> but is trying to be
///     accessed.
/// </summary>
public class BackboardAdapterUserNotFoundException : InvalidOperationException
{
}