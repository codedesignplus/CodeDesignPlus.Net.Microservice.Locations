namespace CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;

public interface ICountryRepository : IRepositoryBase
{
    Task<List<CountryAggregate>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>¿Algún país usa esta moneda?</summary>
    Task<bool> AnyByCurrencyAsync(Guid idCurrency, CancellationToken cancellationToken);

    /// <summary>¿Algún país usa esta zona horaria (se guarda por nombre)?</summary>
    Task<bool> AnyByTimezoneAsync(string timezone, CancellationToken cancellationToken);

    /// <summary>¿Algún país está en esta región (se guarda por nombre)?</summary>
    Task<bool> AnyByRegionAsync(string region, CancellationToken cancellationToken);

    /// <summary>
    /// Guarda el registro nuevo si su id no existe todavía (pendings/589, regla 66).
    /// </summary>
    /// <param name="country">El agregado nuevo.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns><c>false</c> si otro envío del mismo formulario ya lo guardó: el segundo choca contra la clave primaria.</returns>
    Task<bool> CreateIfAbsentAsync(CountryAggregate country, CancellationToken cancellationToken);
}
