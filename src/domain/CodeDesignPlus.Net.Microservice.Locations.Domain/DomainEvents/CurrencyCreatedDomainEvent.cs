namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CurrencyAggregate>(1, "CurrencyCreatedDomainEvent")]
public class CurrencyCreatedDomainEvent(
    Guid aggregateId,
    string name,
    string code,
    string symbol,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;

    public string Code { get; private set; } = code;

    public string Symbol { get; private set; } = symbol;

    public bool IsActive { get; private set; } = isActive;

    public static CurrencyCreatedDomainEvent Create(Guid aggregateId, string name, string code, string symbol, bool isActive)
    {
        return new CurrencyCreatedDomainEvent(aggregateId, name, code, symbol, isActive);
    }
}
