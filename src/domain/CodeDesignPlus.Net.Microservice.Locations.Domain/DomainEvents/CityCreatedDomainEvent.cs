namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CityAggregate>(1, "CityCreatedDomainEvent")]
public class CityCreatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static CityCreatedDomainEvent Create(Guid aggregateId)
    {
        return new CityCreatedDomainEvent(aggregateId);
    }
}
