namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class CountryAggregate(Guid id) : AggregateRootBase(id)
{
    public string Name { get; private set; } = string.Empty;
    public string Alpha2 { get; private set; } = string.Empty;
    public string Alpha3 { get; private set; } = string.Empty;
    public ushort Code { get; private set; } = 0;
    public string? Capital { get; private set; }
    public Guid IdCurrency { get; private set; } = Guid.Empty;
    public string TimeZone { get; private set; } = string.Empty;

    private CountryAggregate(Guid id, string name, string alpha2, string alpha3, ushort code, string? capital, Guid idCurrency, string timeZone, Guid createdBy) : this(id)
    {
        this.Name = name;
        this.Alpha2 = alpha2;
        this.Alpha3 = alpha3;
        this.Code = code;
        this.Capital = capital;
        this.IdCurrency = idCurrency;
        this.TimeZone = timeZone;
        this.IsActive = true;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(CountryCreatedDomainEvent.Create(Id, Name, Alpha2, Alpha3, Code, Capital, IdCurrency, TimeZone, IsActive));
    }

    public static CountryAggregate Create(Guid id, string name, string alpha2, string alpha3, ushort code, string? capital, Guid idCurrency, string timeZone, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.IsNullOrEmpty(alpha2, Errors.CountryAlpha2IsInvalid);
        DomainGuard.IsNullOrEmpty(alpha3, Errors.Alpha3IsInvalid);
        DomainGuard.IsNotInRange((int)code, 1, 999, Errors.CodeRangeIsInvalid);
        DomainGuard.GuidIsEmpty(idCurrency, Errors.IdCurrencyIsInvalid);
        DomainGuard.IsNullOrEmpty(timeZone, Errors.TimeZoneIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        return new CountryAggregate(id, name, alpha2, alpha3, code, capital, idCurrency, timeZone, createdBy);
    }

    public void Update(string name, string alpha2, string alpha3, ushort code, string? capital, Guid idCurrency, string timeZone, bool isActive, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.IsNullOrEmpty(alpha2, Errors.CountryAlpha2IsInvalid);
        DomainGuard.IsNullOrEmpty(alpha3, Errors.Alpha3IsInvalid);
        DomainGuard.IsNotInRange(code, 1, 999, Errors.CodeRangeIsInvalid);
        DomainGuard.GuidIsEmpty(idCurrency, Errors.IdCurrencyIsInvalid);
        DomainGuard.IsNullOrEmpty(timeZone, Errors.TimeZoneIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);

        this.Name = name;
        this.Alpha2 = alpha2;
        this.Alpha3 = alpha3;
        this.Code = code;
        this.Capital = capital;
        this.IdCurrency = idCurrency;
        this.TimeZone = timeZone;
        this.IsActive = isActive;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(CountryUpdatedDomainEvent.Create(Id, Name, Alpha2, Alpha3, Code, Capital, IdCurrency, TimeZone, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsActive = false;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = deletedBy;

        AddEvent(CountryDeletedDomainEvent.Create(Id, Name, Alpha2, Alpha3, Code, Capital, IdCurrency, TimeZone, IsActive));
    }
}
