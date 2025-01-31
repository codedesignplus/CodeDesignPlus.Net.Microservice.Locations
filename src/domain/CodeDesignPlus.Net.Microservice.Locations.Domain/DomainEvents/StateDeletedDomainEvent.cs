namespace CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

[EventKey<StateAggregate>(1, "StateDeletedDomainEvent")]
public class StateDeletedDomainEvent(
     Guid aggregateId,
     Guid? eventId = null,
     Instant? occurredAt = null,
     Dictionary<string, object>? metadata = null
) : DomainEvent(aggregateId, eventId, occurredAt, metadata)
{
    public static StateDeletedDomainEvent Create(Guid aggregateId)
    {
        return new StateDeletedDomainEvent(aggregateId);
    }
}
