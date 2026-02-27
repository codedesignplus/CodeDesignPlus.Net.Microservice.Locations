namespace CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;

public interface ICurrencyRepository : IRepositoryBase
{
    Task<List<CurrencyAggregate>> GetAllAsync(CancellationToken cancellationToken);
}