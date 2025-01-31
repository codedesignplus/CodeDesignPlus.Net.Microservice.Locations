namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindCityById;

public record FindCityByIdQuery(Guid Id) : IRequest<CityDto>;

