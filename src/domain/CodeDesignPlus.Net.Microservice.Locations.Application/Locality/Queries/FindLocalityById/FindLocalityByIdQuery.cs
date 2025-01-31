namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindLocalityById;

public record FindLocalityByIdQuery(Guid Id) : IRequest<LocalityDto>;

