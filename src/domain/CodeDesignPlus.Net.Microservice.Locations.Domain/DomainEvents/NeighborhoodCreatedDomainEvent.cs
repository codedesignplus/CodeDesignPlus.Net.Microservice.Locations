namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<NeighborhoodAggregate>(1, "NeighborhoodCreatedDomainEvent")]
public class NeighborhoodCreatedDomainEvent(
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

    public static NeighborhoodCreatedDomainEvent Create(Guid aggregateId, string name, Guid idLocality, bool isActive)
    {
        return new NeighborhoodCreatedDomainEvent(aggregateId, name, idLocality, isActive);
    }
}
