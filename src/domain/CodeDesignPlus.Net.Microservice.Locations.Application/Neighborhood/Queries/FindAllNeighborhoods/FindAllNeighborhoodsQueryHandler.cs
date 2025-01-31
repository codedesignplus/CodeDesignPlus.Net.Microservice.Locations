namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindAllNeighborhoods;

public class FindAllNeighborhoodsQueryHandler(INeighborhoodRepository repository, IMapper mapper) : IRequestHandler<FindAllNeighborhoodsQuery, List<NeighborhoodDto>>
{
    public async Task<List<NeighborhoodDto>> Handle(FindAllNeighborhoodsQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var neighborhoods = await repository.MatchingAsync<NeighborhoodAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<NeighborhoodDto>>(neighborhoods);
    }
}
