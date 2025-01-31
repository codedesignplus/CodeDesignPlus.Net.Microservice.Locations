namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindCityById;

public class FindCityByIdQueryHandler(ICityRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindCityByIdQuery, CityDto>
{
    public Task<CityDto> Handle(FindCityByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<CityDto>(default!);
    }
}
