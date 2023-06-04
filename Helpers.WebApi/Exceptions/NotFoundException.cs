namespace Helpers.WebApi.Exceptions;

/// <summary>
///     The exception thrown when a resource is not found. Will result in a 404 Not Found response if thrown in a command
///     or query.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public NotFoundException(string name, object key)
        : base($"Entity '{name}' ({key}) was not found.")
    {
    }
}