namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.DataTransferObjects;

public class CountryDto: IDtoBase
{
    public Guid Id { get;  set; } 
    public string Name { get;  set; } = null!;
    public string Alpha2 { get;  set; } = null!;
    public string Alpha3 { get;  set; } = null!;
    public string Code { get;  set; } = null!;
    public string? Capital { get;  set; }
    public Guid IdCurrency { get;  set; } 
    public string Timezone { get;  set; } = null!;
    public string NameNative { get; set; } = null!;
    public string Region { get; set; } = null!;
    public string SubRegion { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Flag { get; set; }
    public bool IsActive { get; set; }
}