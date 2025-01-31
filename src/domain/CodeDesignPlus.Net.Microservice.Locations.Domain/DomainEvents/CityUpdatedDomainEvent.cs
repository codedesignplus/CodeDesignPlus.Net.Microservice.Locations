namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CityAggregate>(1, "CityUpdatedDomainEvent")]
public class CityUpdatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static CityUpdatedDomainEvent Create(Guid aggregateId)
    {
        return new CityUpdatedDomainEvent(aggregateId);
    }
}
