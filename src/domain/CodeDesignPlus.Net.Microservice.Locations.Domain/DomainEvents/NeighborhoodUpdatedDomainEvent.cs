namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<NeighborhoodAggregate>(1, "NeighborhoodUpdatedDomainEvent")]
public class NeighborhoodUpdatedDomainEvent(
    Guid aggregateId,
    string name,
    Guid idLocality,
    bool isActive = true,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;

    public Guid IdLocality { get; private set; } = idLocality;

    public bool IsActive { get; private set; } = isActive;

    public static NeighborhoodUpdatedDomainEvent Create(Guid aggregateId, string name, Guid idLocality, bool isActive = true)
    {
        return new NeighborhoodUpdatedDomainEvent(aggregateId, name, idLocality, isActive);
    }
}
