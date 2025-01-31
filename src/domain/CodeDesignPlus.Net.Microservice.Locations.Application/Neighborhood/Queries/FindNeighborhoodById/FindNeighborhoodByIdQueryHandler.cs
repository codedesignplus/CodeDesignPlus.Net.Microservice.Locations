namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindNeighborhoodById;

public class FindNeighborhoodByIdQueryHandler(INeighborhoodRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindNeighborhoodByIdQuery, NeighborhoodDto>
{
    public Task<NeighborhoodDto> Handle(FindNeighborhoodByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<NeighborhoodDto>(default!);
    }
}
