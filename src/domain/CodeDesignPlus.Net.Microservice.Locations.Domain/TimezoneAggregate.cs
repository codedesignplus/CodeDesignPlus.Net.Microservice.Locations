namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class TimezoneAggregate(Guid id) : AggregateRootBase(id)
{    
    public string Name { get; private set; } = string.Empty;

    private TimezoneAggregate(Guid id, string name, Guid createdBy): this(id)
    {
        this.Name = name;
        this.IsActive = true;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(TimezoneCreatedDomainEvent.Create(Id, Name, IsActive));
    }

    public static TimezoneAggregate Create(Guid id, string name, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        return new TimezoneAggregate(id, name, createdBy);
    }

    public void Update(string name, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);

        this.Name = name;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(TimezoneUpdatedDomainEvent.Create(Id, Name, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsActive = false;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = deletedBy;

        AddEvent(TimezoneDeletedDomainEvent.Create(Id, Name, IsActive));
    }

    
}
