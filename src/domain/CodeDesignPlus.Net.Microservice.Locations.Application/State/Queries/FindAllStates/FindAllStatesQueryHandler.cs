using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindAllStates;

public class FindAllStatesQueryHandler(IStateRepository repository, IMapper mapper) : IRequestHandler<FindAllStatesQuery, Pagination<StateDto>>
{
    public async Task<Pagination<StateDto>> Handle(FindAllStatesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var states = await repository.MatchingAsync<StateAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<StateDto>>(states);
    }
}