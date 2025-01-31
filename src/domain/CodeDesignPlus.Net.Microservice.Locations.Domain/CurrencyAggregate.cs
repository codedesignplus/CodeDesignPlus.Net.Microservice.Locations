namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class CurrencyAggregate(Guid id) : AggregateRoot(id)
{
    public string Code { get; private set; } = string.Empty;
    
    public string Symbol { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;
    
    public static CurrencyAggregate Create(Guid id, Guid tenant, Guid createBy)
    {
       return default;
    }
}
