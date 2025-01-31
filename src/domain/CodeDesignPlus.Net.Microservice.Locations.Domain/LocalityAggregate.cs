namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class LocalityAggregate(Guid id) : AggregateRoot(id)

{
    public Guid IdCity { get; private set; } = Guid.Empty;

    public string Name { get; private set; } = string.Empty;

    public static LocalityAggregate Create(Guid id, Guid tenant, Guid createBy)
    {
       return default;
    }
}
