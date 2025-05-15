using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindAllCities;

public class FindAllCitiesQueryHandler(ICityRepository repository, IMapper mapper) : IRequestHandler<FindAllCitiesQuery, Pagination<CityDto>>
{
    public async Task<Pagination<CityDto>> Handle(FindAllCitiesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        if(string.IsNullOrEmpty(request.Criteria.Filters) || !request.Criteria.Filters.Contains(nameof(CityDto.IdState), StringComparison.OrdinalIgnoreCase))
            return new Pagination<CityDto>([], 0, 0, 0);

        var cities = await repository.MatchingAsync<CityAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<CityDto>>(cities);
    }
}
