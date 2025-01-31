namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class CityCurrencyAggregate(Guid id) : IEntityBase
{
    public Guid Id { get; set; } = id;
    
    public Guid IdCity { get; set; }

    public Guid IdCurrency { get; set; }

    public bool IsMainCurrency { get; set; }

    public string Usage { get; set; } = string.Empty;
}
