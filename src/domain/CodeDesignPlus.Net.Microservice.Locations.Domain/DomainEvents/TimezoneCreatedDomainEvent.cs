using CodeDesignPlus.Net.Microservice.Locations.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<TimezoneAggregate>(1, "TimezoneCreatedDomainEvent")]
public class TimezoneCreatedDomainEvent(
    Guid aggregateId,
    string name, 
    List<string> aliases, 
    Location location, 
    List<string> offsets, 
    string currentOffset, 
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;
    public List<string> Aliases { get; private set; } = aliases;
    public Location Location { get; private set; } = location;  
    public List<string> Offsets { get; private set; } = offsets;
    public string CurrentOffset { get; private set; } = currentOffset;
    public bool IsActive { get; private set; } = isActive;

    public static TimezoneCreatedDomainEvent Create(Guid aggregateId, string name, List<string> aliases, Location location, List<string> offsets, string currentOffset, bool isActive)
    {
        return new TimezoneCreatedDomainEvent(aggregateId, name, aliases, location, offsets, currentOffset, isActive);
    }
}
