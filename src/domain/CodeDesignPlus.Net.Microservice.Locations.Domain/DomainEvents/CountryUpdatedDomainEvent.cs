namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CountryAggregate>(1, "CountryUpdatedDomainEvent")]
public class CountryUpdatedDomainEvent(
    Guid aggregateId,
    string name,
    string alpha2,
    string alpha3,
    string code,
    string? capital,
    Guid idCurrency,
    string timeZone,
    string nameNative,
    string region,
    string subRegion,
    double latitude,
    double longitude,
    string? flag,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public string Name { get; private set; } = name;
    public string Alpha2 { get; private set; } = alpha2;
    public string Alpha3 { get; private set; } = alpha3;
    public string Code { get; private set; } = code;
    public string? Capital { get; private set; } = capital;
    public Guid IdCurrency { get; private set; } = idCurrency;
    public string Timezone { get; private set; } = timeZone;
    public string NameNative { get; private set; } = nameNative;
    public string Region { get; private set; } = region;
    public string SubRegion { get; private set; } = subRegion;
    public double Latitude { get; private set; } = latitude;
    public double Longitude { get; private set; } = longitude;
    public string? Flag { get; private set; } = flag;
    public bool IsActive { get; private set; } = isActive;

    public static CountryUpdatedDomainEvent Create(Guid aggregateId, string name, string alpha2, string alpha3, string code, string? capital, Guid idCurrency, string timeZone, string nameNative, string region, string subRegion, double latitude, double longitude, string? flag, bool isActive)
    {
        return new CountryUpdatedDomainEvent(aggregateId, name, alpha2, alpha3, code, capital, idCurrency, timeZone, nameNative, region, subRegion, latitude, longitude, flag, isActive);
    }
}