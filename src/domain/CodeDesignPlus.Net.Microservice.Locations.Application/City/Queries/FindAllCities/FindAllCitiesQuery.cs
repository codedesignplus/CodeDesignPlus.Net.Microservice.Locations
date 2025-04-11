using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindAllCities;

public record FindAllCitiesQuery(C.Criteria Criteria) : IRequest<Pagination<CityDto>>;

