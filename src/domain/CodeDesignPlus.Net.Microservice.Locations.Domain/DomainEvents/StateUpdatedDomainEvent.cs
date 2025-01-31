namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<StateAggregate>(1, "StateUpdatedDomainEvent")]
public class StateUpdatedDomainEvent(
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

    public static StateUpdatedDomainEvent Create(Guid aggregateId, Guid idCountry, string code, string name, bool isActive)
    {
        return new StateUpdatedDomainEvent(aggregateId, idCountry, code, name, isActive);
    }
}
