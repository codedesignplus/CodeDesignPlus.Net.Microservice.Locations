namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<StateAggregate>(1, "StateCreatedDomainEvent")]
public class StateCreatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static StateCreatedDomainEvent Create(Guid aggregateId)
    {
        return new StateCreatedDomainEvent(aggregateId);
    }
}
