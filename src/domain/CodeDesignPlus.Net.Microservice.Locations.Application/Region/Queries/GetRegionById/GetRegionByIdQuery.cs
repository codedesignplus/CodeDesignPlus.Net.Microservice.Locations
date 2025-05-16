namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Queries.GetRegionById;

public record GetRegionByIdQuery(Guid Id) : IRequest<RegionDto>;

