namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Controllers;

/// <summary>
/// Service for managing the Regions.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class RegionController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Regions.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Regions.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Regions.</returns>
    [HttpGet]
    public async Task<IActionResult> GetRegions([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllRegionsQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a Region by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Region.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Region.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRegionById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRegionByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new Region.
    /// </summary>
    /// <param name="data">Data for creating the Region.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateRegionCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Region.
    /// </summary>
    /// <param name="id">The unique identifier of the Region.</param>
    /// <param name="data">Data for updating the Region.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRegion(Guid id, [FromBody] UpdateRegionDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<UpdateRegionCommand>(data), cancellationToken);

        return NoContent();
    }
}