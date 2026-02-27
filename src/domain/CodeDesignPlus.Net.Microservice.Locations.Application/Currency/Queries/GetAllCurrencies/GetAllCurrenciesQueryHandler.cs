using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.GetAllCurrencies;

public class GetAllCurrenciesQueryHandler(ICurrencyRepository repository, IMapper mapper, ICacheManager cache) : IRequestHandler<GetAllCurrenciesQuery, Pagination<CurrencyDto>>
{
    public const string Key = "GetAllCurrenciesQuery";

    public async Task<Pagination<CurrencyDto>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        if (!string.IsNullOrEmpty(request.Criteria.Filters))
            return await SearchCountryAsync(request, cancellationToken);

        return await GetAllAsync(cancellationToken);
    }

    private async Task<Pagination<CurrencyDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var exists = await cache.ExistsAsync(Key);

        if (exists)
            return await cache.GetAsync<Pagination<CurrencyDto>>(Key);

        var currencies = await repository.GetAllAsync(cancellationToken);

        var currencyDtos = mapper.Map<Pagination<CurrencyDto>>(Pagination<CurrencyAggregate>.Create(currencies, currencies.Count, currencies.Count, 0));

        await cache.SetAsync(Key, currencyDtos, TimeSpan.FromHours(6));

        return currencyDtos;
    }

    private async Task<Pagination<CurrencyDto>> SearchCountryAsync(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var currencies = await repository.MatchingAsync<CurrencyAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<CurrencyDto>>(currencies);
    }
}