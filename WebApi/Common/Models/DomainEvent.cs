namespace WebApi.Common;

public abstract class DomainEvent
{
    protected DomainEvent()
    {
        DateOccurred = DateTimeOffset.UtcNow;
    }

    public bool IsPublished { get; set; }
    public DateTimeOffset DateOccurred { get; protected set; }
}

public interface IHasDomainEvent
{
    public List<DomainEvent> DomainEvents { get; }
}