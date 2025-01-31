namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetCountryById;

public record GetCountryByIdQuery(Guid Id) : IRequest<CountryDto>;

