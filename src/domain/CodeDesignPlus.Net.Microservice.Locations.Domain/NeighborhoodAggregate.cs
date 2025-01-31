namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class NeighborhoodAggregate(Guid id) : AggregateRoot(id)
{    
    public Guid IdLocality { get; private set; } = Guid.Empty;

    public string Name { get; private set; } = string.Empty;

    private NeighborhoodAggregate(Guid id, Guid idLocality, string name, Guid createdBy) : this(id)
    {
        this.IdLocality = idLocality;
        this.Name = name;
        this.IsActive = true;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(NeighborhoodCreatedDomainEvent.Create(Id, Name, IdLocality, IsActive));
    }

    public static NeighborhoodAggregate Create(Guid id, Guid idLocality, string name, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.GuidIsEmpty(idLocality, Errors.IdLocalityIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        return new NeighborhoodAggregate(id, idLocality, name, createdBy);
    }

    public void Update(Guid idLocality, string name, Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(idLocality, Errors.IdLocalityIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);

        this.IdLocality = idLocality;
        this.Name = name;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(NeighborhoodUpdatedDomainEvent.Create(Id, Name, IdLocality, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsActive = false;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = deletedBy;

        AddEvent(NeighborhoodDeletedDomainEvent.Create(Id, Name, IdLocality, IsActive));
    }
}
