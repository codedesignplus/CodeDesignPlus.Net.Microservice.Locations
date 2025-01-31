namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.DataTransferObjects;

public class NeighborhoodDto: IDtoBase
{
    public required Guid Id { get; set; }
    public Guid IdLocality { get; set; }
    public required string Name { get; set; }
}