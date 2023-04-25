using System.Runtime.Serialization;

namespace WebApi.Common.Exceptions;

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