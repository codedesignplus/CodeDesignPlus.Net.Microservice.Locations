namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetCountryById;

public class GetCountryByIdQueryHandler(ICountryRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<GetCountryByIdQuery, CountryDto>
{
    public const string Key = "GetCountryByIdQuery:{0}";
    public async Task<CountryDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var cacheKey = string.Format(Key, request.Id);

        var exists = await cacheManager.ExistsAsync(cacheKey);

        if (exists)
            return await cacheManager.GetAsync<CountryDto>(cacheKey);

        var country = await repository.FindAsync<CountryAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(country, Errors.CountryNotFound);

        await cacheManager.SetAsync(cacheKey, mapper.Map<CountryDto>(country));

        return mapper.Map<CountryDto>(country);
    }
}
