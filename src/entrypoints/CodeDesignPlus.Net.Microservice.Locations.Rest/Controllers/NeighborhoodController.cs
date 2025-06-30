using Microsoft.AspNetCore.Authorization;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NeighborhoodController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Neighborhoods.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Neighborhoods.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Neighborhoods.</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetNeighborhoods([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindAllNeighborhoodsQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a Neighborhood by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Neighborhood.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Neighborhood.</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetNeighborhoodById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindNeighborhoodByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new Neighborhood.
    /// </summary>
    /// <param name="data">Data for creating the Neighborhood.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateNeighborhood([FromBody] CreateNeighborhoodDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateNeighborhoodCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Neighborhood.
    /// </summary>
    /// <param name="id">The unique identifier of the Neighborhood.</param>
    /// <param name="data">Data for updating the Neighborhood.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNeighborhood(Guid id, [FromBody] UpdateNeighborhoodDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<UpdateNeighborhoodCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing Neighborhood.
    /// </summary>
    /// <param name="id">The unique identifier of the Neighborhood.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNeighborhood(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteNeighborhoodCommand(id), cancellationToken);

        return NoContent();
    }
}