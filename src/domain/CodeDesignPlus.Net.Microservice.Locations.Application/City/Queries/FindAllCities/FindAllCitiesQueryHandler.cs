namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindAllCities;

public class FindAllCitiesQueryHandler(ICityRepository repository, IMapper mapper) : IRequestHandler<FindAllCitiesQuery, List<CityDto>>
{
    public async Task<List<CityDto>> Handle(FindAllCitiesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var cities = await repository.MatchingAsync<CityAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<CityDto>>(cities);
    }
}
