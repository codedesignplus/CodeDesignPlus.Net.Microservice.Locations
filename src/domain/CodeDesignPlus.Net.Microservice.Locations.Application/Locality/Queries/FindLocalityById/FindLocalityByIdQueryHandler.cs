namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindLocalityById;

public class FindLocalityByIdQueryHandler(ILocalityRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindLocalityByIdQuery, LocalityDto>
{
    public Task<LocalityDto> Handle(FindLocalityByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<LocalityDto>(default!);
    }
}
