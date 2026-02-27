using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.GetAllCurrencies;

public record GetAllCurrenciesQuery(C.Criteria Criteria) : IRequest<Pagination<CurrencyDto>>;

