namespace WebApi.Infrastructure.Persistence;

public class TimestampedEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}