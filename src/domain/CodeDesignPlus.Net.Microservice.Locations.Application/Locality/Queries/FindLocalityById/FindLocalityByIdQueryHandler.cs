namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindLocalityById;

public class FindLocalityByIdQueryHandler(ILocalityRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<FindLocalityByIdQuery, LocalityDto>
{
    public async Task<LocalityDto> Handle(FindLocalityByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<LocalityDto>(request.Id.ToString());

        var locality = await repository.FindAsync<LocalityAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(locality, Errors.LocalityNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<LocalityDto>(locality));

        return mapper.Map<LocalityDto>(locality);
    }
}