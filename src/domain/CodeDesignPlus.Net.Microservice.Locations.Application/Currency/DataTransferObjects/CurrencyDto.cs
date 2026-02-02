namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.DataTransferObjects;

public class CurrencyDto : IDtoBase
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required short NumericCode { get; set; }
    public required short DecimalDigits { get; set; }
    public required string Symbol { get; set; }
    public required string Name { get; set; }
    public bool IsActive { get; set; }
}