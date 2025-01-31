namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<LocalityAggregate>(1, "LocalityDeletedDomainEvent")]
public class LocalityDeletedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static LocalityDeletedDomainEvent Create(Guid aggregateId)
    {
        return new LocalityDeletedDomainEvent(aggregateId);
    }
}
