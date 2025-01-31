namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindAllLocalities;

public record FindAllLocalitiesQuery(Guid Id) : IRequest<LocalityDto>;

