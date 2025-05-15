using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindAllStates;

public class FindAllStatesQueryHandler(IStateRepository repository, IMapper mapper) : IRequestHandler<FindAllStatesQuery, Pagination<StateDto>>
{
    public async Task<Pagination<StateDto>> Handle(FindAllStatesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        if(string.IsNullOrEmpty(request.Criteria.Filters) || !request.Criteria.Filters.Contains(nameof(StateDto.IdCountry), StringComparison.OrdinalIgnoreCase))
            return new Pagination<StateDto>([], 0, 0, 0);

        var states = await repository.MatchingAsync<StateAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<StateDto>>(states);
    }
}