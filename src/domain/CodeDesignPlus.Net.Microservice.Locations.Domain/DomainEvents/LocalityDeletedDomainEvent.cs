namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<LocalityAggregate>(1, "LocalityDeletedDomainEvent")]
public class LocalityDeletedDomainEvent(
    Guid aggregateId,
    string name,
    Guid idCity,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;

    public Guid IdCity { get; private set; } = idCity;

    public bool IsActive { get; private set; } = isActive;
    
    public static LocalityDeletedDomainEvent Create(Guid aggregateId, string name, Guid idCity, bool isActive)
    {
        return new LocalityDeletedDomainEvent(aggregateId, name, idCity, isActive);
    }
}
