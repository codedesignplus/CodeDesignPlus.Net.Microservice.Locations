namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<NeighborhoodAggregate>(1, "NeighborhoodCreatedDomainEvent")]
public class NeighborhoodCreatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static NeighborhoodCreatedDomainEvent Create(Guid aggregateId)
    {
        return new NeighborhoodCreatedDomainEvent(aggregateId);
    }
}
