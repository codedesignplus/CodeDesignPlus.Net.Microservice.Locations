namespace CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;

public interface ICityRepository : IRepositoryBase
{
    /// <summary>¿Hay alguna ciudad en este estado?</summary>
    Task<bool> AnyByStateAsync(Guid idState, CancellationToken cancellationToken);

    /// <summary>¿Alguna ciudad usa esta zona horaria (se guarda por nombre)?</summary>
    Task<bool> AnyByTimezoneAsync(string timezone, CancellationToken cancellationToken);

    /// <summary>
    /// Guarda el registro nuevo si su id no existe todavía (pendings/589, regla 66).
    /// </summary>
    /// <param name="city">El agregado nuevo.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns><c>false</c> si otro envío del mismo formulario ya lo guardó: el segundo choca contra la clave primaria.</returns>
    Task<bool> CreateIfAbsentAsync(CityAggregate city, CancellationToken cancellationToken);
}
