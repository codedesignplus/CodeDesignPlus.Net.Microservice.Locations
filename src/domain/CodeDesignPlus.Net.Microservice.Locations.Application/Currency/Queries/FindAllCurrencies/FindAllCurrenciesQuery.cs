using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindAllCurrencies;

public record FindAllCurrenciesQuery(C.Criteria Criteria) : IRequest<Pagination<CurrencyDto>>;

