namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetCountryById;

public class GetCountryByIdQueryHandler(ICountryRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<GetCountryByIdQuery, CountryDto>
{
    public Task<CountryDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<CountryDto>(default!);
    }
}
