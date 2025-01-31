namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<TimezoneAggregate>(1, "TimezoneDeletedDomainEvent")]
public class TimezoneDeletedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static TimezoneDeletedDomainEvent Create(Guid aggregateId)
    {
        return new TimezoneDeletedDomainEvent(aggregateId);
    }
}
