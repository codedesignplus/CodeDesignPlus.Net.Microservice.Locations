using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindAllCities;

public class FindAllCitiesQueryHandler(ICityRepository repository, IMapper mapper) : IRequestHandler<FindAllCitiesQuery, Pagination<CityDto>>
{
    public async Task<Pagination<CityDto>> Handle(FindAllCitiesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var cities = await repository.MatchingAsync<CityAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<CityDto>>(cities);
    }
}
