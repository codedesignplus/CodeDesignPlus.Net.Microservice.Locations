namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.DataTransferObjects;

public class CountryDto: IDtoBase
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Code { get; set; }

    public Guid IdCurrency { get; set; } 

    public required string TimeZone { get; set; } 
}