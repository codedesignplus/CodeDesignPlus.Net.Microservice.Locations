namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindNeighborhoodById;

public record FindNeighborhoodByIdQuery(Guid Id) : IRequest<NeighborhoodDto>;

