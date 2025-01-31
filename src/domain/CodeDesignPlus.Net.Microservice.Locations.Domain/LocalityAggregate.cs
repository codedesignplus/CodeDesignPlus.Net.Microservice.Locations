namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class LocalityAggregate(Guid id) : AggregateRootBase(id)

{
    public Guid IdCity { get; private set; } = Guid.Empty;

    public string Name { get; private set; } = string.Empty;

    private LocalityAggregate(Guid id, Guid idCity, string name, Guid createdBy) : this(id)
    {
        this.IdCity = idCity;
        this.Name = name;
        this.IsActive = true;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(LocalityCreatedDomainEvent.Create(Id, Name, IdCity, IsActive));
    }

    public static LocalityAggregate Create(Guid id, Guid idCity, string name, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.GuidIsEmpty(idCity, Errors.IdCityIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        return new LocalityAggregate(id, idCity, name, createdBy);
    }

    public void Update(Guid idCity, string name, bool isActive,Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(idCity, Errors.IdCityIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);

        this.IdCity = idCity;
        this.Name = name;
        this.IsActive = isActive;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(LocalityUpdatedDomainEvent.Create(Id, Name, IdCity, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsActive = false;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = deletedBy;

        AddEvent(LocalityDeletedDomainEvent.Create(Id, Name, IdCity, IsActive));
    }
}
