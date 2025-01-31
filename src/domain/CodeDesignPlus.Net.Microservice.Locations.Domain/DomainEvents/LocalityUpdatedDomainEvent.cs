namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<LocalityAggregate>(1, "LocalityUpdatedDomainEvent")]
public class LocalityUpdatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static LocalityUpdatedDomainEvent Create(Guid aggregateId)
    {
        return new LocalityUpdatedDomainEvent(aggregateId);
    }
}
