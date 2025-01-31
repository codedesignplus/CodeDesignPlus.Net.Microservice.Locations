namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindAllLocalities;

public class FindAllLocalitiesQueryHandler(ILocalityRepository repository, IMapper mapper) : IRequestHandler<FindAllLocalitiesQuery, List<LocalityDto>>
{
    public async Task<List<LocalityDto>> Handle(FindAllLocalitiesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var localities = await repository.MatchingAsync<LocalityAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<LocalityDto>>(localities);
    }
}