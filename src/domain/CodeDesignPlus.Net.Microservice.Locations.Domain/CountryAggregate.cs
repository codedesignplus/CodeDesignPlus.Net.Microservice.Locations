namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class CountryAggregate(Guid id) : AggregateRoot(id)
{
    public string Name { get; private set; } = string.Empty;

    public string Code { get; private set; } = string.Empty;

    public Guid IdCurrency { get; private set; } = Guid.Empty;

    public string TimeZone { get; private set; } = string.Empty;

    private CountryAggregate(Guid id, string name, string code, Guid idCurrency, string timeZone, Guid createdBy) : this(id)
    {
        this.Name = name;
        this.Code = code;
        this.IdCurrency = idCurrency;
        this.TimeZone = timeZone;
        this.IsActive = true;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(CountryCreatedDomainEvent.Create(Id, Name, Code, IdCurrency, TimeZone, IsActive));
    }

    public static CountryAggregate Create(Guid id, string name, string code, Guid idCurrency, string timeZone, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.IsNullOrEmpty(code, Errors.CountryCodeIsInvalid);
        DomainGuard.GuidIsEmpty(idCurrency, Errors.IdCurrencyIsInvalid);
        DomainGuard.IsNullOrEmpty(timeZone, Errors.TimeZoneIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        return new CountryAggregate(id, name, code, idCurrency, timeZone, createdBy);
    }

    public void Update(string name, string code, Guid idCurrency, string timeZone, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.IsNullOrEmpty(code, Errors.CountryCodeIsInvalid);
        DomainGuard.GuidIsEmpty(idCurrency, Errors.IdCurrencyIsInvalid);
        DomainGuard.IsNullOrEmpty(timeZone, Errors.TimeZoneIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);

        this.Name = name;
        this.Code = code;
        this.IdCurrency = idCurrency;
        this.TimeZone = timeZone;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(CountryUpdatedDomainEvent.Create(Id, Name, Code, IdCurrency, TimeZone, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsActive = false;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = deletedBy;

        AddEvent(CountryDeletedDomainEvent.Create(Id, Name, Code, IdCurrency, TimeZone, IsActive));
    }
}
