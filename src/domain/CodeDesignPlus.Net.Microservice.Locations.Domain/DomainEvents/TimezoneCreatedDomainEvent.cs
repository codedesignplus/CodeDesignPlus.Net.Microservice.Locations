namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<TimezoneAggregate>(1, "TimezoneCreatedDomainEvent")]
public class TimezoneCreatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static TimezoneCreatedDomainEvent Create(Guid aggregateId)
    {
        return new TimezoneCreatedDomainEvent(aggregateId);
    }
}
