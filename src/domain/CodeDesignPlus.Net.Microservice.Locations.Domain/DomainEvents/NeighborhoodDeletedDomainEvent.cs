namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<NeighborhoodAggregate>(1, "NeighborhoodDeletedDomainEvent")]
public class NeighborhoodDeletedDomainEvent(
    Guid aggregateId,
    string name,
    Guid idLocality,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;

    public Guid IdLocality { get; private set; } = idLocality;

    public bool IsActive { get; private set; } = isActive;

    public static NeighborhoodDeletedDomainEvent Create(Guid aggregateId, string name, Guid idLocality, bool isActive)
    {
        return new NeighborhoodDeletedDomainEvent(aggregateId, name, idLocality, isActive);
    }
}
