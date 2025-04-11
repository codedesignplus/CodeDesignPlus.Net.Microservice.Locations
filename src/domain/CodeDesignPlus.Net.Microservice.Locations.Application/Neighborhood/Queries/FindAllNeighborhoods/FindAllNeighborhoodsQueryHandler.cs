using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindAllNeighborhoods;

public class FindAllNeighborhoodsQueryHandler(INeighborhoodRepository repository, IMapper mapper) : IRequestHandler<FindAllNeighborhoodsQuery, Pagination<NeighborhoodDto>>
{
    public async Task<Pagination<NeighborhoodDto>> Handle(FindAllNeighborhoodsQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var neighborhoods = await repository.MatchingAsync<NeighborhoodAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<NeighborhoodDto>>(neighborhoods);
    }
}
