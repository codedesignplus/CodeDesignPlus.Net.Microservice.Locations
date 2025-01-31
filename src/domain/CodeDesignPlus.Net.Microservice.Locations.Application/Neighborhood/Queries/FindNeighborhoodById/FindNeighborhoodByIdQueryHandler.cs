namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindNeighborhoodById;

public class FindNeighborhoodByIdQueryHandler(INeighborhoodRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<FindNeighborhoodByIdQuery, NeighborhoodDto>
{
    public async Task<NeighborhoodDto> Handle(FindNeighborhoodByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<NeighborhoodDto>(request.Id.ToString());

        var neighborhood = await repository.FindAsync<NeighborhoodAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(neighborhood, Errors.NeighborhoodNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<NeighborhoodDto>(neighborhood));

        return mapper.Map<NeighborhoodDto>(neighborhood);
    }
}