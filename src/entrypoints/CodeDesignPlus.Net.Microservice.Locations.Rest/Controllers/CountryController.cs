using Microsoft.AspNetCore.Authorization;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Controllers;

/// <summary>
/// Service for managing the Countries.
/// </summary>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class CountryController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get all Countries.
    /// </summary>
    /// <param name="criteria">Criteria for filtering and sorting the Countries.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of Countries.</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCountries([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllCountryQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a Country by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Country.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Country.</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCountryById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCountryByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Create a new Country.
    /// </summary>
    /// <param name="data">Data for creating the Country.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPost]
    public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto data, CancellationToken cancellationToken)
    { 
        await mediator.Send(mapper.Map<CreateCountryCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Update an existing Country.
    /// </summary>
    /// <param name="id">The unique identifier of the Country.</param>
    /// <param name="data">Data for updating the Country.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCountry(Guid id, [FromBody] UpdateCountryDto data, CancellationToken cancellationToken)
    {
        data.Id = id;

        await mediator.Send(mapper.Map<UpdateCountryCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Delete a Country by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Country.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>HTTP status code 204 (No Content).</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCountryCommand(id), cancellationToken);

        return NoContent();
    }
}
