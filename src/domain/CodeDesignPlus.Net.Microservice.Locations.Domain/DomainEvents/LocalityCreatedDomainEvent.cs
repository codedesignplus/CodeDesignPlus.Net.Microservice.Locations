namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<LocalityAggregate>(1, "LocalityCreatedDomainEvent")]
public class LocalityCreatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static LocalityCreatedDomainEvent Create(Guid aggregateId)
    {
        return new LocalityCreatedDomainEvent(aggregateId);
    }
}
