namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<NeighborhoodAggregate>(1, "NeighborhoodDeletedDomainEvent")]
public class NeighborhoodDeletedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static NeighborhoodDeletedDomainEvent Create(Guid aggregateId)
    {
        return new NeighborhoodDeletedDomainEvent(aggregateId);
    }
}
