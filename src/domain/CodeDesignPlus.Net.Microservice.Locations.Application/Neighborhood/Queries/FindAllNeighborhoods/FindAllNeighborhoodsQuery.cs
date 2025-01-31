namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindAllNeighborhoods;

public record FindAllNeighborhoodsQuery(C.Criteria Criteria) : IRequest<NeighborhoodDto>;

