using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;

public class GetAllCountryQueryHandler(ICountryRepository repository, IMapper mapper, ICacheManager cache) : IRequestHandler<GetAllCountryQuery, Pagination<CountryDto>>
{
    public const string Key = "GetAllCountryQuery";

    public async Task<Pagination<CountryDto>> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        if (!string.IsNullOrEmpty(request.Criteria.Filters))
            return await SearchCountryAsync(request, cancellationToken);

        return await GetAllAsync(cancellationToken);
    }

    private async Task<Pagination<CountryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cachedCountries = await cache.GetAsync<Pagination<CountryDto>>(Key);

        if (cachedCountries != null)
            return cachedCountries;

        var countries = await repository.GetAllAsync(cancellationToken);

        var dtos = mapper.Map<Pagination<CountryDto>>(Pagination<CountryAggregate>.Create(countries, countries.Count, countries.Count, 0));

        await cache.SetAsync(Key, dtos, TimeSpan.FromHours(6));

        return dtos;
    }

    private async Task<Pagination<CountryDto>> SearchCountryAsync(GetAllCountryQuery request, CancellationToken cancellationToken)
    {
        var countries = await repository.MatchingAsync<CountryAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<CountryDto>>(countries);
    }
}