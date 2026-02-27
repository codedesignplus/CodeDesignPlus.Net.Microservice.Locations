namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class CountryRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<CountryRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ICountryRepository
{
   
    public Task<List<CountryAggregate>> GetAllAsync(CancellationToken cancellationToken)
    {
        var filter = Builders<CountryAggregate>.Filter.Eq(x => x.IsActive, true);

        return GetCollection<CountryAggregate>().Find(filter).ToListAsync(cancellationToken);
    }
}