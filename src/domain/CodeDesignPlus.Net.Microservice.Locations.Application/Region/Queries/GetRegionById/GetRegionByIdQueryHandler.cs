namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Queries.GetRegionById;

public class GetRegionByIdQueryHandler(IRegionRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<GetRegionByIdQuery, RegionDto>
{
    public async Task<RegionDto> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<RegionDto>(request.Id.ToString());

        var region = await repository.FindAsync<RegionAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(region, Errors.RegionNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<RegionDto>(region));

        return mapper.Map<RegionDto>(region);
    }
}
