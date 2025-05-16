namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class CityAggregate(Guid id) : AggregateRootBase(id)
{
    
    public Guid IdState { get; private set; } = Guid.Empty;

    public string Name { get; private set; } = string.Empty;

    public string TimeZone { get; private set; } = string.Empty;

    private CityAggregate(Guid id, Guid idState, string name, string timeZone, Guid createdBy): this(id)
    {
        this.IdState = idState;
        this.Name = name;
        this.TimeZone = timeZone;
        this.IsActive = true;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(CityCreatedDomainEvent.Create(Id, IdState, Name, TimeZone, IsActive));
    }

    public static CityAggregate Create(Guid id, Guid idState, string name, string timeZone, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.GuidIsEmpty(idState, Errors.IdStateIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        return new CityAggregate(id, idState, name, timeZone, createdBy);
    }

    public void Update(Guid idState, string name, string timeZone, bool isActive, Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(idState, Errors.IdStateIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);

        this.IdState = idState;
        this.Name = name;
        this.TimeZone = timeZone;
        this.IsActive = isActive;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(CityUpdatedDomainEvent.Create(Id, IdState, Name, TimeZone, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsActive = false;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = deletedBy;

        AddEvent(CityDeletedDomainEvent.Create(Id, IdState, Name, TimeZone, IsActive));
    }
}
