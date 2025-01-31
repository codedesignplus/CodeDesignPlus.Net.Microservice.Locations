namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<StateAggregate>(1, "StateUpdatedDomainEvent")]
public class StateUpdatedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static StateUpdatedDomainEvent Create(Guid aggregateId)
    {
        return new StateUpdatedDomainEvent(aggregateId);
    }
}
