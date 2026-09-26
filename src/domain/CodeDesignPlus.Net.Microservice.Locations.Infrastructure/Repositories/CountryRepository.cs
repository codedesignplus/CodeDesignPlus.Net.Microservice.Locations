namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class CountryRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<CountryRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ICountryRepository
{
   
    public Task<List<CountryAggregate>> GetAllAsync(CancellationToken cancellationToken)
    {
        var filter = Builders<CountryAggregate>.Filter.Eq(x => x.IsActive, true);

        return GetCollection<CountryAggregate>().Find(filter).ToListAsync(cancellationToken);
    }

    public Task<bool> AnyByCurrencyAsync(Guid idCurrency, CancellationToken cancellationToken)
        => GetCollection<CountryAggregate>().Find(x => x.IdCurrency == idCurrency).AnyAsync(cancellationToken);

    public Task<bool> AnyByTimezoneAsync(string timezone, CancellationToken cancellationToken)
        => GetCollection<CountryAggregate>().Find(x => x.Timezone == timezone).AnyAsync(cancellationToken);

    public Task<bool> AnyByRegionAsync(string region, CancellationToken cancellationToken)
        => GetCollection<CountryAggregate>().Find(x => x.Region == region).AnyAsync(cancellationToken);
}
