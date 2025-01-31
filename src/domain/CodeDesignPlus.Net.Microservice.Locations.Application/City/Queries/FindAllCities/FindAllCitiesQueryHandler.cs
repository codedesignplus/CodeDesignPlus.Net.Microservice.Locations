namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindAllCities;

public class FindAllCitiesQueryHandler(ICityRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindAllCitiesQuery, CityDto>
{
    public Task<CityDto> Handle(FindAllCitiesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<CityDto>(default!);
    }
}
