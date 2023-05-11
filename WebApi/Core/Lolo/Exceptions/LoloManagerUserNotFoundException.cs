namespace WebApi.Core.Lolo.Exceptions;

/// <summary>
///     The exception thrown when the user is not found in <see cref="LoloManager" /> but is trying to be accessed.
/// </summary>
public class LoloManagerUserNotFoundException : InvalidOperationException
{
}