using Blueboard.Infrastructure.Persistence.Entities;

namespace Blueboard.Core.Backboard.Exceptions;

/// <summary>
///     The exception thrown when the encrypted json contents of a <see cref="GradeImport" /> are not decryptable or are in
///     an invalid json structure.
/// </summary>
public class InvalidImportException : Exception
{
}