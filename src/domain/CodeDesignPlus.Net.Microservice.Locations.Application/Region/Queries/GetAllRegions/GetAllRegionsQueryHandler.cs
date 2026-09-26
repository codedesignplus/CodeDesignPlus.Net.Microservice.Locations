using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Queries.GetAllRegions;

/// <summary>
/// Sin caché a propósito. Se cacheaba con una clave fija ("Regions") sin mirar el criterio, así que la primera consulta
/// (una página, un filtro, un límite) se servía a todas las demás durante una hora, y una región nueva no aparecía
/// (plan 037 de pendings). Son seis registros: consultar Mongo cuesta menos que mantener bien esa caché.
/// </summary>
public class GetAllRegionsQueryHandler(IRegionRepository repository, IMapper mapper) : IRequestHandler<GetAllRegionsQuery, Pagination<RegionDto>>
{
    public async Task<Pagination<RegionDto>> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var regions = await repository.MatchingAsync<RegionAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<RegionDto>>(regions);
    }
}
