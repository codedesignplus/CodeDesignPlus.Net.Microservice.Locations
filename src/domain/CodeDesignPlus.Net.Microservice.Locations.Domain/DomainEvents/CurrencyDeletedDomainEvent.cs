namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CurrencyAggregate>(1, "CurrencyDeletedDomainEvent")]
public class CurrencyDeletedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static CurrencyDeletedDomainEvent Create(Guid aggregateId)
    {
        return new CurrencyDeletedDomainEvent(aggregateId);
    }
}
