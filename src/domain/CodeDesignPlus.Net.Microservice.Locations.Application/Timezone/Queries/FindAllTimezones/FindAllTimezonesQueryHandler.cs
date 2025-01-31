namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindAllTimezones;

public class FindAllTimezonesQueryHandler(ITimezoneRepository repository, IMapper mapper) : IRequestHandler<FindAllTimezonesQuery, List<TimezoneDto>>
{
    public async Task<List<TimezoneDto>> Handle(FindAllTimezonesQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var timezones = await repository.MatchingAsync<TimezoneAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<TimezoneDto>>(timezones);
    }
}