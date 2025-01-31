namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;

public record GetAllCountryQuery(Guid Id) : IRequest<CountryDto>;

