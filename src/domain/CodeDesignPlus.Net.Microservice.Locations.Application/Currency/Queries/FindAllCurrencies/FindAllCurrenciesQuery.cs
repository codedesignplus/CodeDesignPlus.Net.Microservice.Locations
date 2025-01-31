namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindAllCurrencies;

public record FindAllCurrenciesQuery(C.Criteria Criteria) : IRequest<List<CurrencyDto>>;

