namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CountryAggregate>(1, "CountryCreatedDomainEvent")]
public class CountryCreatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static CountryCreatedDomainEvent Create(Guid aggregateId)
    {
        return new CountryCreatedDomainEvent(aggregateId);
    }
}
