namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class StateAggregate(Guid id) : AggregateRoot(id)
{    
    public Guid IdCountry { get; private set; } = Guid.Empty;

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public static StateAggregate Create(Guid id, Guid tenant, Guid createBy)
    {
       return default;
    }
}
