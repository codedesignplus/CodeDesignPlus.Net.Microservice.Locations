namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindAllNeighborhoods;

public class FindAllNeighborhoodsQueryHandler(INeighborhoodRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindAllNeighborhoodsQuery, NeighborhoodDto>
{
    public Task<NeighborhoodDto> Handle(FindAllNeighborhoodsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<NeighborhoodDto>(default!);
    }
}
