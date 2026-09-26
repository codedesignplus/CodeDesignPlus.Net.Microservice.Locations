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
}
