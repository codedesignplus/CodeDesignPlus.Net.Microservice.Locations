namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.DataTransferObjects;

public class TimezoneDto: IDtoBase
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; } 
    public bool IsActive { get; set; }
}