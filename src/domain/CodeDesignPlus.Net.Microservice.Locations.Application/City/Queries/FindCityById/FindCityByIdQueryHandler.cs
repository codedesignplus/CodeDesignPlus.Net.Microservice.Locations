namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindCityById;

public class FindCityByIdQueryHandler(ICityRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<FindCityByIdQuery, CityDto>
{
    public async Task<CityDto> Handle(FindCityByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<CityDto>(request.Id.ToString());

        var city = await repository.FindAsync<CityAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(city, Errors.CityNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<CityDto>(city));

        return mapper.Map<CityDto>(city);
    }
}
