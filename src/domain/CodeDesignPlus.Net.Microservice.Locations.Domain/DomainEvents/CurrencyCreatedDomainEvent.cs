namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CurrencyAggregate>(1, "CurrencyCreatedDomainEvent")]
public class CurrencyCreatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static CurrencyCreatedDomainEvent Create(Guid aggregateId)
    {
        return new CurrencyCreatedDomainEvent(aggregateId);
    }
}
