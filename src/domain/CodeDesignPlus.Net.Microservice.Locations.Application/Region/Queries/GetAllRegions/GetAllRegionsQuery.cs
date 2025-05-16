using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Queries.GetAllRegions;

public record GetAllRegionsQuery(C.Criteria Criteria) : IRequest<Pagination<RegionDto>>;

