namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CountryAggregate>(1, "CountryDeletedDomainEvent")]
public class CountryDeletedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static CountryDeletedDomainEvent Create(Guid aggregateId)
    {
        return new CountryDeletedDomainEvent(aggregateId);
    }
}
