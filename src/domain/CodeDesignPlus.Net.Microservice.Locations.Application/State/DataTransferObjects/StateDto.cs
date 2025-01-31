namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.DataTransferObjects;

public class StateDto: IDtoBase
{
    public required Guid Id { get; set; }
    
    public Guid IdCountry { get;  set; }

    public required string Code { get;  set; } 

    public required string Name { get;  set; } 
}