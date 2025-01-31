namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CountryAggregate>(1, "CountryCreatedDomainEvent")]
public class CountryCreatedDomainEvent(
    Guid aggregateId,
    string name,
    string code,
    Guid idCurrency,
    string timeZone,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;

    public string Code { get; private set; } = code;

    public Guid IdCurrency { get; private set; } = idCurrency;

    public string TimeZone { get; private set; } = timeZone;

    public bool IsActive { get; private set; } = isActive;

    public static CountryCreatedDomainEvent Create(Guid aggregateId, string name, string code, Guid idCurrency, string timeZone, bool isActive)
    {
        return new CountryCreatedDomainEvent(aggregateId, name, code, idCurrency, timeZone, isActive);
    }
}
