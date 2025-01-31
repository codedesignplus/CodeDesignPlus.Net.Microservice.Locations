namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindAllStates;

public class FindAllStatesQueryHandler(IStateRepository repository, IMapper mapper) : IRequestHandler<FindAllStatesQuery, List<StateDto>>
{
    public async Task<List<StateDto>> Handle(FindAllStatesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var states = await repository.MatchingAsync<StateAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<StateDto>>(states);
    }
}