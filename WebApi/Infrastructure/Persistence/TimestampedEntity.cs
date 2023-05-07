namespace WebApi.Infrastructure.Persistence;

/// <summary>
///     The base class for all entities that have timestamps. The timestamps are automatically updated by
///     <see cref="ApplicationDbContext" /> when not using bulk operations.
/// </summary>
public class TimestampedEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}