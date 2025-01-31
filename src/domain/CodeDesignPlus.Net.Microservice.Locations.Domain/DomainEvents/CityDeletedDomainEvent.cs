namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CityAggregate>(1, "CityDeletedDomainEvent")]
public class CityDeletedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static CityDeletedDomainEvent Create(Guid aggregateId)
    {
        return new CityDeletedDomainEvent(aggregateId);
    }
}
