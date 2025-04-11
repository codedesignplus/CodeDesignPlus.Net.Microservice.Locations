using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindAllNeighborhoods;

public record FindAllNeighborhoodsQuery(C.Criteria Criteria) : IRequest<Pagination<NeighborhoodDto>>;

