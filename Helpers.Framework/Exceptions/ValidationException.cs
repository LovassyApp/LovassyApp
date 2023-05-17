using FluentValidation.Results;

namespace Helpers.Framework.Exceptions;

/// <summary>
///     The exception thrown when a validation error occurs. Will result in a 400 Bad Request response if thrown in a
///     command or query.
/// </summary>
public class ValidationException : Exception
{
    private ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}