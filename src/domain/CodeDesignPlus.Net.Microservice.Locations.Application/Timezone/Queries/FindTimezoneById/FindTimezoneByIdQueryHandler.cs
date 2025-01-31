namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindTimezoneById;

public class FindTimezoneByIdQueryHandler(ITimezoneRepository repository, IMapper mapper, ICacheManager cacheManager): IRequestHandler<FindTimezoneByIdQuery, TimezoneDto>
{
    public async Task<TimezoneDto> Handle(FindTimezoneByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<TimezoneDto>(request.Id.ToString());

        var timezone = await repository.FindAsync<TimezoneAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(timezone, Errors.TimezoneNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<TimezoneDto>(timezone));

        return mapper.Map<TimezoneDto>(timezone);
    }
}
