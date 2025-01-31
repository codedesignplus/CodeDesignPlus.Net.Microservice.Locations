namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<NeighborhoodAggregate>(1, "NeighborhoodUpdatedDomainEvent")]
public class NeighborhoodUpdatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static NeighborhoodUpdatedDomainEvent Create(Guid aggregateId)
    {
        return new NeighborhoodUpdatedDomainEvent(aggregateId);
    }
}
