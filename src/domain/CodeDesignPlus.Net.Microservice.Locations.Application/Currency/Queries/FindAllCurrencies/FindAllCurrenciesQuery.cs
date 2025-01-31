namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindAllCurrencies;

public record FindAllCurrenciesQuery(Guid Id) : IRequest<CurrencyDto>;

