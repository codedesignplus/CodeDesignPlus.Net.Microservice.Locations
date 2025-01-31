namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<TimezoneAggregate>(1, "TimezoneUpdatedDomainEvent")]
public class TimezoneUpdatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static TimezoneUpdatedDomainEvent Create(Guid aggregateId)
    {
        return new TimezoneUpdatedDomainEvent(aggregateId);
    }
}
