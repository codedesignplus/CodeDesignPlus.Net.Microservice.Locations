namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindStateById;

public class FindStateByIdQueryHandler(IStateRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindStateByIdQuery, StateDto>
{
    public Task<StateDto> Handle(FindStateByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<StateDto>(default!);
    }
}
