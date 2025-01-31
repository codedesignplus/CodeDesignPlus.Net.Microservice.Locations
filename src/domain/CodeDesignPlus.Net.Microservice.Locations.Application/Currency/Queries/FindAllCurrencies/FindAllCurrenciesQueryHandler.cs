namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindAllCurrencies;

public class FindAllCurrenciesQueryHandler(ICurrencyRepository repository, IMapper mapper) : IRequestHandler<FindAllCurrenciesQuery, List<CurrencyDto>>
{
    public async Task<List<CurrencyDto>> Handle(FindAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var currencies = await repository.MatchingAsync<CurrencyAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<CurrencyDto>>(currencies);
    }
}