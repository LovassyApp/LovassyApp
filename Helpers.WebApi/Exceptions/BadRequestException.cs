using System.Runtime.Serialization;

namespace Helpers.WebApi.Exceptions;

/// <summary>
///     The exception thrown when we want to return a bad request status code (400) but we don't have validation errors.
/// </summary>
public class BadRequestException : Exception
{
    public BadRequestException()
    {
    }

    public BadRequestException(string? message) : base(message)
    {
    }

    public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}