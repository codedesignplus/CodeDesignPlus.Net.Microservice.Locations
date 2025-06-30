using Microsoft.AspNetCore.Authorization;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Controllers;

/// <summary>
/// Service for managing the States.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class StateController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all States.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the States.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of States.</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetStates([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindAllStatesQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a State by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the State.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The State.</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStateById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindStateByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new State.
    /// </summary>
    /// <param name="data">Data for creating the State.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateState([FromBody] CreateStateDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateStateCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing State.
    /// </summary>
    /// <param name="id">The unique identifier of the State.</param>
    /// <param name="data">Data for updating the State.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateState(Guid id, [FromBody] UpdateStateDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<UpdateStateCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing State.
    /// </summary>
    /// <param name="id">The unique identifier of the State.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteState(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteStateCommand(id), cancellationToken);

        return NoContent();
    }
}