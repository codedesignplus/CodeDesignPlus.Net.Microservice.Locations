namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Controllers;

/// <summary>
/// Service for managing Timezones.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class LocalityController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Localities.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Localities.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Localities.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLocalities([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindAllLocalitiesQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a Locality by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Locality.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Locality.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLocalityById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindLocalityByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new Locality.
    /// </summary>
    /// <param name="data">Data for creating the Locality.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateLocality([FromBody] CreateLocalityDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateLocalityCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Locality.
    /// </summary>
    /// <param name="id">The unique identifier of the Locality.</param>
    /// <param name="data">Data for updating the Locality.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocality(Guid id, [FromBody] UpdateLocalityDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<UpdateLocalityCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing Locality.
    /// </summary>
    /// <param name="id">The unique identifier of the Locality.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocality(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteLocalityCommand(id), cancellationToken);

        return NoContent();
    }
}