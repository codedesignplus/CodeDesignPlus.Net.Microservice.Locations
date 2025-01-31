namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<LocalityAggregate>(1, "LocalityUpdatedDomainEvent")]
public class LocalityUpdatedDomainEvent(
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
    
    public static LocalityUpdatedDomainEvent Create(Guid aggregateId, string name, Guid idCity, bool isActive)
    {
        return new LocalityUpdatedDomainEvent(aggregateId, name, idCity, isActive);
    }
}
