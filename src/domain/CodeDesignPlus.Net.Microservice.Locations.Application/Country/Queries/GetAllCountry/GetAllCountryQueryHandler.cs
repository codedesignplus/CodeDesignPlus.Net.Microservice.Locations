using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;

public class GetAllCountryQueryHandler(ICountryRepository repository, IMapper mapper) : IRequestHandler<GetAllCountryQuery, Pagination<CountryDto>>
{
    public async Task<Pagination<CountryDto>> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var countries = await repository.MatchingAsync<CountryAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<CountryDto>>(countries);
    }
}