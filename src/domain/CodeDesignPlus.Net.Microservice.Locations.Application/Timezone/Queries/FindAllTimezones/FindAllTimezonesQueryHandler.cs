namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindAllTimezones;

public class FindAllTimezonesQueryHandler(ITimezoneRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindAllTimezonesQuery, TimezoneDto>
{
    public Task<TimezoneDto> Handle(FindAllTimezonesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<TimezoneDto>(default!);
    }
}
