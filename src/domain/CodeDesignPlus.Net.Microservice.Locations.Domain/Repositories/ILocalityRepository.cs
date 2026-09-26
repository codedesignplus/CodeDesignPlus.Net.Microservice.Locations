namespace CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;

public interface ILocalityRepository : IRepositoryBase
{
    /// <summary>¿Hay alguna localidad en esta ciudad?</summary>
    Task<bool> AnyByCityAsync(Guid idCity, CancellationToken cancellationToken);
}
