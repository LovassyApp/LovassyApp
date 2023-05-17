using System.Runtime.Serialization;

namespace Helpers.Framework.Exceptions;

/// <summary>
///     The exception thrown when a service (for example an endpoint) is not available. Will result in a 503 Service
///     Unavailable response if thrown
///     in a command or query.
/// </summary>
public class UnavailableException : Exception
{
    public UnavailableException()
    {
    }

    public UnavailableException(string? message) : base(message)
    {
    }

    public UnavailableException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}