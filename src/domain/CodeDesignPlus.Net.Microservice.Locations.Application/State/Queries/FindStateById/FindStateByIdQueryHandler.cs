namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindStateById;

public class FindStateByIdQueryHandler(IStateRepository repository, IMapper mapper, ICacheManager cacheManager) : IRequestHandler<FindStateByIdQuery, StateDto>
{
    public async Task<StateDto> Handle(FindStateByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exists = await cacheManager.ExistsAsync(request.Id.ToString());

        if (exists)
            return await cacheManager.GetAsync<StateDto>(request.Id.ToString());

        var state = await repository.FindAsync<StateAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(state, Errors.StateNotFound);

        await cacheManager.SetAsync(request.Id.ToString(), mapper.Map<StateDto>(state));

        return mapper.Map<StateDto>(state);
    }
}
