namespace CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;

public interface INeighborhoodRepository : IRepositoryBase
{
    /// <summary>¿Hay algún barrio en esta localidad?</summary>
    Task<bool> AnyByLocalityAsync(Guid idLocality, CancellationToken cancellationToken);
}
