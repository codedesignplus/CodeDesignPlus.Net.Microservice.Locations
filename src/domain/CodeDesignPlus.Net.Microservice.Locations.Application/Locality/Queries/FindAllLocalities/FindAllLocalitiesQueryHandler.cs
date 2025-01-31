namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindAllLocalities;

public class FindAllLocalitiesQueryHandler(ILocalityRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindAllLocalitiesQuery, LocalityDto>
{
    public Task<LocalityDto> Handle(FindAllLocalitiesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<LocalityDto>(default!);
    }
}
