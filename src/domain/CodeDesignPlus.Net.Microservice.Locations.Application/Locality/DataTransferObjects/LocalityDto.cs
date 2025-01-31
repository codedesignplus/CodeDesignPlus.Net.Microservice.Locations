namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.DataTransferObjects;

public class LocalityDto : IDtoBase
{
    public required Guid Id { get; set; }
    public Guid IdCity { get; set; }
    public required string Name { get; set; }
}