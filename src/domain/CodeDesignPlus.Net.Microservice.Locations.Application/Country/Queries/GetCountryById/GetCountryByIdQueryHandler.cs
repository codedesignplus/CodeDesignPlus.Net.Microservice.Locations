namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetCountryById;

public class GetCountryByIdQueryHandler(ICountryRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<GetCountryByIdQuery, CountryDto>
{
    public async Task<CountryDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<CountryDto>(request.Id.ToString());

        var country = await repository.FindAsync<CountryAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(country, Errors.CountryNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<CountryDto>(country));

        return mapper.Map<CountryDto>(country);
    }
}
