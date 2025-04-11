namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.DataTransferObjects;

public class CityDto: IDtoBase
{
    public required Guid Id { get; set; }

    public required Guid IdState { get; set; }

    public required string Name { get; set; }

    public required string TimeZone { get; set; }
    public bool IsActive { get; set; }
}