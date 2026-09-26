namespace CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;

public interface IStateRepository : IRepositoryBase
{
    /// <summary>¿Hay algún estado en este país?</summary>
    Task<bool> AnyByCountryAsync(Guid idCountry, CancellationToken cancellationToken);
}
