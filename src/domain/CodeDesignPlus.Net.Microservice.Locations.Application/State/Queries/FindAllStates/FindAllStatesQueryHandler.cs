namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindAllStates;

public class FindAllStatesQueryHandler(IStateRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindAllStatesQuery, StateDto>
{
    public Task<StateDto> Handle(FindAllStatesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<StateDto>(default!);
    }
}
