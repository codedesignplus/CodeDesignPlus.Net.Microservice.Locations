using Microsoft.AspNetCore.Authorization;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Controllers;

/// <summary>
/// Service for managing the Timezones.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class TimezoneController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Timezones.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Timezones.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Timezones.</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetTimezones([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindAllTimezonesQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a Timezone by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Timezone.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Timezone.</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTimezoneById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindTimezoneByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new Timezone.
    /// </summary>
    /// <param name="data">Data for creating the Timezone.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateTimezone([FromBody] CreateTimezoneDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateTimezoneCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Timezone.
    /// </summary>
    /// <param name="id">The unique identifier of the Timezone.</param>
    /// <param name="data">Data for updating the Timezone.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTimezone(Guid id, [FromBody] UpdateTimezoneDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<UpdateTimezoneCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing Timezone.
    /// </summary>
    /// <param name="id">The unique identifier of the Timezone.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTimezone(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteTimezoneCommand(id), cancellationToken);

        return NoContent();
    }
}