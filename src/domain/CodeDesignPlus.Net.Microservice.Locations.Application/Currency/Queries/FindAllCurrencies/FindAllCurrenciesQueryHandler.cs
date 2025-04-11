using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindAllCurrencies;

public class FindAllCurrenciesQueryHandler(ICurrencyRepository repository, IMapper mapper) : IRequestHandler<FindAllCurrenciesQuery, Pagination<CurrencyDto>>
{
    public async Task<Pagination<CurrencyDto>> Handle(FindAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var currencies = await repository.MatchingAsync<CurrencyAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<CurrencyDto>>(currencies);
    }
}