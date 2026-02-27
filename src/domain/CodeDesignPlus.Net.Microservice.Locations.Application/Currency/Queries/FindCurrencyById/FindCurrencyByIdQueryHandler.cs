namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindCurrencyById;

public class FindCurrencyByIdQueryHandler(ICurrencyRepository repository, IMapper mapper, ICacheManager cacheManager): IRequestHandler<FindCurrencyByIdQuery, CurrencyDto>
{

    public const string Key = "FindCurrencyByIdQuery:{0}";

    public async Task<CurrencyDto> Handle(FindCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var cacheKey = string.Format(Key, request.Id);

        var exists = await cacheManager.ExistsAsync(cacheKey);

        if (exists)
            return await cacheManager.GetAsync<CurrencyDto>(cacheKey);

        var currency = await repository.FindAsync<CurrencyAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(currency, Errors.CurrencyNotFound);

        await cacheManager.SetAsync(cacheKey, mapper.Map<CurrencyDto>(currency));

        return mapper.Map<CurrencyDto>(currency);
    }
}