namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CurrencyAggregate>(1, "CurrencyUpdatedDomainEvent")]
public class CurrencyUpdatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static CurrencyUpdatedDomainEvent Create(Guid aggregateId)
    {
        return new CurrencyUpdatedDomainEvent(aggregateId);
    }
}
