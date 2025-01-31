namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindTimezoneById;

public class FindTimezoneByIdQueryHandler(ITimezoneRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindTimezoneByIdQuery, TimezoneDto>
{
    public Task<TimezoneDto> Handle(FindTimezoneByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<TimezoneDto>(default!);
    }
}
