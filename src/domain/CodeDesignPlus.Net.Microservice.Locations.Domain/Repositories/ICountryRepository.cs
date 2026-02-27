namespace CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;

public interface ICountryRepository : IRepositoryBase
{
    Task<List<CountryAggregate>> GetAllAsync(CancellationToken cancellationToken);
}