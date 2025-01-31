namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class NeighborhoodAggregate(Guid id) : AggregateRoot(id)
{
    
    public Guid IdLocality { get; private set; } = Guid.Empty;

    public string Name { get; private set; } = string.Empty;

    public static NeighborhoodAggregate Create(Guid id, Guid tenant, Guid createBy)
    {
       return default;
    }
}
