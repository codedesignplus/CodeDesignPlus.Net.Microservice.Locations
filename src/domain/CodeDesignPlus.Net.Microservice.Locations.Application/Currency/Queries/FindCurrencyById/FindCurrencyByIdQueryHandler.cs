namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindCurrencyById;

public class FindCurrencyByIdQueryHandler(ICurrencyRepository repository, IMapper mapper, ICacheManager cacheManager): IRequestHandler<FindCurrencyByIdQuery, CurrencyDto>
{
    public async Task<CurrencyDto> Handle(FindCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<CurrencyDto>(request.Id.ToString());

        var currency = await repository.FindAsync<CurrencyAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(currency, Errors.CurrencyNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<CurrencyDto>(currency));

        return mapper.Map<CurrencyDto>(currency);
    }
}