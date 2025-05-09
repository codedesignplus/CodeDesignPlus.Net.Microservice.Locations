namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<StateAggregate>(1, "StateDeletedDomainEvent")]
public class StateDeletedDomainEvent(
    Guid aggregateId,
    Guid idCountry,
    string code,
    string name,
    bool isActive,
    Guid? eventId = null,
    Instant? occurredAt = null,
    Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public Guid IdCountry { get; private set; } = idCountry;

    public string Code { get; private set; } = code;

    public string Name { get; private set; } = name;

    public bool IsActive { get; private set; } = isActive;

    public static StateDeletedDomainEvent Create(Guid aggregateId, Guid idCountry, string code, string name, bool isActive)
    {
        return new StateDeletedDomainEvent(aggregateId, idCountry, code, name, isActive);
    }
}
