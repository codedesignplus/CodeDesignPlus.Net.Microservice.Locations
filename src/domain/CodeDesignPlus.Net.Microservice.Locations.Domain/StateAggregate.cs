namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class StateAggregate(Guid id) : AggregateRoot(id)
{
    public Guid IdCountry { get; private set; } = Guid.Empty;

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    private StateAggregate(Guid id, Guid idCountry, string code, string name, Guid createdBy) : this(id)
    {
        this.IdCountry = idCountry;
        this.Code = code;
        this.Name = name;
        this.IsActive = true;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(StateCreatedDomainEvent.Create(Id, IdCountry, Code, Name, IsActive));
    }

    public static StateAggregate Create(Guid id, Guid idCountry, string code, string name, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.GuidIsEmpty(idCountry, Errors.IdCountryIsInvalid);
        DomainGuard.IsNullOrEmpty(code, Errors.StateCodeIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        return new StateAggregate(id, idCountry, code, name, createdBy);
    }

    public void Update(Guid idCountry, string code, string name, bool isActive, Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(idCountry, Errors.IdCountryIsInvalid);
        DomainGuard.IsNullOrEmpty(code, Errors.StateCodeIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);

        this.IdCountry = idCountry;
        this.Code = code;
        this.Name = name;
        this.IsActive = isActive;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(StateUpdatedDomainEvent.Create(Id, IdCountry, Code, Name, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsActive = false;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = deletedBy;

        AddEvent(StateDeletedDomainEvent.Create(Id, IdCountry, Code, Name, IsActive));
    }
}
