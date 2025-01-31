namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindAllCities;

public record FindAllCitiesQuery(C.Criteria Criteria) : IRequest<List<CityDto>>;

