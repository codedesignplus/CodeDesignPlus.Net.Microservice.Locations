namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class CountryAggregate(Guid id) : AggregateRootBase(id)
{
    public string Name { get; private set; } = null!;
    public string Alpha2 { get; private set; } = null!;
    public string Alpha3 { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public string? Capital { get; private set; }
    public Guid IdCurrency { get; private set; } = Guid.Empty;
    public string Timezone { get; private set; } = null!;
    public string NameNative { get; private set; } = null!;
    public string Region { get; private set; } = null!;
    public string SubRegion { get; private set; } = null!;
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string? Flag { get; private set; }

    private CountryAggregate(Guid id, string name, string alpha2, string alpha3, string code, string? capital, Guid idCurrency, string timeZone, string nameNative, string region, string subRegion, double latitude, double longitude, string? flag, bool isActive, Guid createdBy) : this(id)
    {
        this.Name = name;
        this.Alpha2 = alpha2;
        this.Alpha3 = alpha3;
        this.Code = code;
        this.Capital = capital;
        this.IdCurrency = idCurrency;
        this.Timezone = timeZone;
        this.NameNative = nameNative;
        this.Region = region;
        this.SubRegion = subRegion;
        this.Latitude = latitude;
        this.Longitude = longitude;
        this.Flag = flag;
        this.IsActive = isActive;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(CountryCreatedDomainEvent.Create(Id, Name, Alpha2, Alpha3, Code, Capital, IdCurrency, Timezone, NameNative, Region, SubRegion, Latitude, Longitude, Flag, IsActive));
    }

    public static CountryAggregate Create(Guid id, string name, string alpha2, string alpha3, string code, string? capital, Guid idCurrency, string timeZone, string nameNative, string region, string subRegion, double latitude, double longitude, string? flag, bool isActive, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.IsNullOrEmpty(alpha2, Errors.CountryAlpha2IsInvalid);
        DomainGuard.IsNullOrEmpty(alpha3, Errors.Alpha3IsInvalid);
        DomainGuard.IsNullOrEmpty(code, Errors.NumericCodeIsInvalid);
        DomainGuard.GuidIsEmpty(idCurrency, Errors.IdCurrencyIsInvalid);
        DomainGuard.IsNullOrEmpty(timeZone, Errors.TimezoneIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);
        DomainGuard.IsNullOrEmpty(nameNative, Errors.NameNativeIsInvalid);
        DomainGuard.IsNullOrEmpty(region, Errors.RegionIsInvalid);
        DomainGuard.IsNullOrEmpty(subRegion, Errors.SubRegionIsInvalid);
        DomainGuard.IsNotInRange(latitude, -90, 90, Errors.LatitudeIsInvalid);
        DomainGuard.IsNotInRange(longitude, -180, 180, Errors.LongitudeIsInvalid);

        return new CountryAggregate(id, name, alpha2, alpha3, code, capital, idCurrency, timeZone, nameNative, region, subRegion, latitude, longitude, flag, isActive, createdBy);
    }

    public void Update(string name, string alpha2, string alpha3, string code, string? capital, Guid idCurrency, string timeZone,string nameNative, string region, string subRegion, double latitude, double longitude, string? flag, bool isActive, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.IsNullOrEmpty(alpha2, Errors.CountryAlpha2IsInvalid);
        DomainGuard.IsNullOrEmpty(alpha3, Errors.Alpha3IsInvalid);
        DomainGuard.IsNullOrEmpty(code, Errors.NumericCodeIsInvalid);
        DomainGuard.GuidIsEmpty(idCurrency, Errors.IdCurrencyIsInvalid);
        DomainGuard.IsNullOrEmpty(timeZone, Errors.TimezoneIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);
        DomainGuard.IsNullOrEmpty(nameNative, Errors.NameNativeIsInvalid);
        DomainGuard.IsNullOrEmpty(region, Errors.RegionIsInvalid);
        DomainGuard.IsNullOrEmpty(subRegion, Errors.SubRegionIsInvalid);
        DomainGuard.IsNotInRange(latitude, -90, 90, Errors.LatitudeIsInvalid);
        DomainGuard.IsNotInRange(longitude, -180, 180, Errors.LongitudeIsInvalid);

        this.Name = name;
        this.Alpha2 = alpha2;
        this.Alpha3 = alpha3;
        this.Code = code;
        this.Capital = capital;
        this.IdCurrency = idCurrency;
        this.Timezone = timeZone;
        this.NameNative = nameNative;
        this.Region = region;
        this.SubRegion = subRegion;
        this.Latitude = latitude;
        this.Longitude = longitude;
        this.Flag = flag;
        this.IsActive = isActive;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(CountryUpdatedDomainEvent.Create(Id, Name, Alpha2, Alpha3, Code, Capital, IdCurrency, Timezone, NameNative, Region, SubRegion, Latitude, Longitude, Flag, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsActive = false;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = deletedBy;

        AddEvent(CountryDeletedDomainEvent.Create(Id, Name, Alpha2, Alpha3, Code, Capital, IdCurrency, Timezone, NameNative, Region, SubRegion, Latitude, Longitude, Flag, IsActive));
    }
}
