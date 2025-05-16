using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Queries.GetAllRegions;

public class GetAllRegionsQueryHandler(IRegionRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<GetAllRegionsQuery, Pagination<RegionDto>>
{
    public async Task<Pagination<RegionDto>> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var cacheKey = $"Regions";

        var cacheValue = await cacheManager.GetAsync<Pagination<RegionDto>>(cacheKey);

        if (cacheValue is not null)
            return cacheValue;

        var regions = await repository.MatchingAsync<RegionAggregate>(request.Criteria, cancellationToken);

        var data = mapper.Map<Pagination<RegionDto>>(regions);

        await cacheManager.SetAsync(cacheKey, data, TimeSpan.FromHours(1));
        
        return data;
    }
}
