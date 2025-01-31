namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Controllers;

/// <summary>
/// Controller for managing the Cities.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class CityController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Cities.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Cities.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Cities.</returns>
    [HttpGet]
    public async Task<IActionResult> GetCities([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindAllCitiesQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a City by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the City.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The City.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCityById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindCityByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new City.
    /// </summary>
    /// <param name="data">Data for creating the City.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateCityCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing City.
    /// </summary>
    /// <param name="id">The unique identifier of the City.</param>
    /// <param name="data">Data for updating the City.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCity(Guid id, [FromBody] UpdateCityDto data, CancellationToken cancellationToken)
    {
        data.Id = id;

        await mediator.Send(mapper.Map<UpdateCityCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing City.
    /// </summary>
    /// <param name="id">The unique identifier of the City.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCityCommand(id), cancellationToken);

        return NoContent();
    }
}