namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;

public record GetAllCountryQuery(C.Criteria Criteria) : IRequest<CountryDto>;

