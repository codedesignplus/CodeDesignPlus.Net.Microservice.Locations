namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<TimezoneAggregate>(1, "TimezoneUpdatedDomainEvent")]
public class TimezoneUpdatedDomainEvent(
    Guid aggregateId,
    string name,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;

    public bool IsActive { get; private set; } = isActive;
    
    public static TimezoneUpdatedDomainEvent Create(Guid aggregateId, string name, bool isActive)
    {
        return new TimezoneUpdatedDomainEvent(aggregateId, name, isActive);
    }
}
