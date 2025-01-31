namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class CountryAggregate(Guid id) : AggregateRoot(id)
{
    public string Name { get; private set; } = string.Empty;

    public string Code { get; private set; } = string.Empty;

    public Guid IdCurrency { get; private set; } = Guid.Empty;

    public string TimeZone { get; private set; } = string.Empty;


    public static CountryAggregate Create(Guid id, Guid tenant, Guid createBy)
    {
       return default;
    }
}
