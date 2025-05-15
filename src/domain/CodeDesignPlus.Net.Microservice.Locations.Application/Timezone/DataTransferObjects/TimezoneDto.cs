using CodeDesignPlus.Net.Microservice.Locations.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.DataTransferObjects;

public class TimezoneDto: IDtoBase
{
    public required Guid Id { get; set; }    
    public required string Name { get; set; } 
    public List<string> Aliases { get; set; } = [];
    public Location Location { get; set; } = null!;
    public List<string> Offsets { get; set; } = [];
    public string CurrentOffset { get; set; }= null!;
    public bool IsActive { get; set; }
}