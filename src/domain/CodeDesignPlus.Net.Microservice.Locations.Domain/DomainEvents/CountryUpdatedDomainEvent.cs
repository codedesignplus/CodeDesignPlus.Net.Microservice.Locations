namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CountryAggregate>(1, "CountryUpdatedDomainEvent")]
public class CountryUpdatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static CountryUpdatedDomainEvent Create(Guid aggregateId)
    {
        return new CountryUpdatedDomainEvent(aggregateId);
    }
}
