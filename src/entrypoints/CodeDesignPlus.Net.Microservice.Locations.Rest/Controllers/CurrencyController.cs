namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Controllers;

/// <summary>
/// Service for managing the Currencies.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class CurrencyController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Currencies.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Currencies.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Currencies.</returns>
    [HttpGet]
    public async Task<IActionResult> GetCurrencies([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindAllCurrenciesQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a Currency by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Currency.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Currency.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCurrencyById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindCurrencyByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new Currency.
    /// </summary>
    /// <param name="data">Data for creating the Currency.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateCurrency([FromBody] CreateCurrencyDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateCurrencyCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Currency.
    /// </summary>
    /// <param name="id">The unique identifier of the Currency.</param>
    /// <param name="data">Data for updating the Currency.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCurrency(Guid id, [FromBody] UpdateCurrencyDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<UpdateCurrencyCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete an existing Currency.
    /// </summary>
    /// <param name="id">The unique identifier of the Currency.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCurrency(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCurrencyCommand(id), cancellationToken);

        return NoContent();
    }
}