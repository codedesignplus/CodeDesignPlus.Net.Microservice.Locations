namespace CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;

public interface ILocalityRepository : IRepositoryBase
{
    /// <summary>¿Hay alguna localidad en esta ciudad?</summary>
    Task<bool> AnyByCityAsync(Guid idCity, CancellationToken cancellationToken);

    /// <summary>
    /// Guarda el registro nuevo si su id no existe todavía (pendings/589, regla 66).
    /// </summary>
    /// <param name="locality">El agregado nuevo.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns><c>false</c> si otro envío del mismo formulario ya lo guardó: el segundo choca contra la clave primaria.</returns>
    Task<bool> CreateIfAbsentAsync(LocalityAggregate locality, CancellationToken cancellationToken);
}
