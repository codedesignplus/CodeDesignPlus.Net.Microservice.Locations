namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class CityAggregate(Guid id) : AggregateRootBase(id)
{
    
    public Guid IdState { get; private set; } = Guid.Empty;

    public string Name { get; private set; } = string.Empty;

    public string TimeZone { get; private set; } = string.Empty;


    public static CityAggregate Create(Guid id, Guid tenant, Guid createBy)
    {
       return default;
    }
}
