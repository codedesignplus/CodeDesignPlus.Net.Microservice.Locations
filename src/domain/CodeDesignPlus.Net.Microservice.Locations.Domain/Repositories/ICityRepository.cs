namespace CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;

public interface ICityRepository : IRepositoryBase
{
    /// <summary>¿Hay alguna ciudad en este estado?</summary>
    Task<bool> AnyByStateAsync(Guid idState, CancellationToken cancellationToken);

    /// <summary>¿Alguna ciudad usa esta zona horaria (se guarda por nombre)?</summary>
    Task<bool> AnyByTimezoneAsync(string timezone, CancellationToken cancellationToken);
}
