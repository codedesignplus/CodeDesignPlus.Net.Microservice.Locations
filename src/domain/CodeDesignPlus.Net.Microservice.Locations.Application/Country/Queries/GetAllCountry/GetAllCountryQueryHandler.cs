namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;

public class GetAllCountryQueryHandler(ICountryRepository repository, IMapper mapper) : IRequestHandler<GetAllCountryQuery, List<CountryDto>>
{
    public async Task<List<CountryDto>> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var countries = await repository.MatchingAsync<CountryAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<List<CountryDto>>(countries);
    }
}