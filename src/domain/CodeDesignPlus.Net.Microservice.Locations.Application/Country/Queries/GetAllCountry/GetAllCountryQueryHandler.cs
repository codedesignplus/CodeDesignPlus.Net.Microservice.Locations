namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;

public class GetAllCountryQueryHandler(ICountryRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<GetAllCountryQuery, CountryDto>
{
    public Task<CountryDto> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<CountryDto>(default!);
    }
}
