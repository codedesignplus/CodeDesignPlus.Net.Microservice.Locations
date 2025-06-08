namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<CityAggregate>(1, "CityDeletedDomainEvent")]
public class CityDeletedDomainEvent(
    Guid aggregateId,
    Guid idState,
    string name,
    string timeZone,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public Guid IdState { get; private set; } = idState;

    public string Name { get; private set; } = name;

    public string Timezone { get; private set; } = timeZone;

    public bool IsActive { get; private set; } = isActive;

    public static CityDeletedDomainEvent Create(Guid aggregateId, Guid idState, string name, string timeZone, bool isActive)
    {
        return new CityDeletedDomainEvent(aggregateId, idState, name, timeZone, isActive);
    }
}
