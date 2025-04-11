using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindAllLocalities;

public class FindAllLocalitiesQueryHandler(ILocalityRepository repository, IMapper mapper) : IRequestHandler<FindAllLocalitiesQuery, Pagination<LocalityDto>>
{
    public async Task<Pagination<LocalityDto>> Handle(FindAllLocalitiesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var localities = await repository.MatchingAsync<LocalityAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<LocalityDto>>(localities);
    }
}