namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindAllLocalities;

public record FindAllLocalitiesQuery(C.Criteria Criteria) : IRequest<List<LocalityDto>>;

