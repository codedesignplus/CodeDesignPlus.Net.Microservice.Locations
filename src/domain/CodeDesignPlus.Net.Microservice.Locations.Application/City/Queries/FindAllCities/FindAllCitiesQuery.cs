namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindAllCities;

public record FindAllCitiesQuery(Guid Id) : IRequest<CityDto>;

