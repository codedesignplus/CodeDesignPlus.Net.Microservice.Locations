namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class TimezoneAggregate(Guid id) : AggregateRoot(id)
{    
    public string Name { get; private set; } = string.Empty;

    public static TimezoneAggregate Create(Guid id, Guid tenant, Guid createBy)
    {
       return default;
    }
}
