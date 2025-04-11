using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindAllTimezones;

public class FindAllTimezonesQueryHandler(ITimezoneRepository repository, IMapper mapper) : IRequestHandler<FindAllTimezonesQuery, Pagination<TimezoneDto>>
{
    public async Task<Pagination<TimezoneDto>> Handle(FindAllTimezonesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var timezones = await repository.MatchingAsync<TimezoneAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<TimezoneDto>>(timezones);
    }
}