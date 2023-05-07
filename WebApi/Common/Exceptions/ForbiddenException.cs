using System.Runtime.Serialization;

namespace WebApi.Common.Exceptions;

/// <summary>
///     The exception thrown when a user tries to access a resource they are not allowed to access. Will result in a 403
///     Forbidden response if thrown in a command or query.
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException()
    {
    }

    public ForbiddenException(string? message) : base(message)
    {
    }

    public ForbiddenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}