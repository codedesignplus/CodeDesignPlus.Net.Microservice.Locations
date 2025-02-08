namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.DataTransferObjects;

public class CountryDto: IDtoBase
{
    public Guid Id { get;  set; } 
    public string Name { get;  set; } = null!;
    public string Alpha2 { get;  set; } = null!;
    public string Alpha3 { get;  set; } = null!;
    public ushort Code { get;  set; }
    public string? Capital { get;  set; }
    public Guid IdCurrency { get;  set; } 
    public string TimeZone { get;  set; } = null!;
}