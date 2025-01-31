namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CountryAggregate>(1, "CountryCreatedDomainEvent")]
public class CountryCreatedDomainEvent(
    Guid aggregateId,
    string name,
    string alpha2,
    string alpha3,
    ushort code,
    string? capital,
    Guid idCurrency,
    string timeZone,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;

    public string Alpha2 { get; private set; } = alpha2;

    public string Alpha3 { get; private set; } = alpha3;

    public ushort Code { get; private set; } = code;

    public string? Capital { get; private set; } = capital;

    public Guid IdCurrency { get; private set; } = idCurrency;

    public string TimeZone { get; private set; } = timeZone;

    public bool IsActive { get; private set; } = isActive;


    public static CountryCreatedDomainEvent Create(Guid aggregateId, string name, string alpha2, string alpha3, ushort code, string? capital, Guid idCurrency, string timeZone, bool isActive)
    {
        return new CountryCreatedDomainEvent(aggregateId, name, alpha2, alpha3, code, capital, idCurrency, timeZone, isActive);
    }
}
