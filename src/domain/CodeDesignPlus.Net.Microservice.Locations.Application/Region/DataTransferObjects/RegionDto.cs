namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.DataTransferObjects;

public class RegionDto : IDtoBase
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required List<string> SubRegions { get; set; } = [];
    public required bool IsActive { get; set; }
}