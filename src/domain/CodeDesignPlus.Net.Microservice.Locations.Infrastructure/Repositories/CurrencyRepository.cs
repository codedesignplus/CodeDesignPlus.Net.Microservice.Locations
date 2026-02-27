namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class CurrencyRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<CurrencyRepository> logger)
    : RepositoryBase(serviceProvider, mongoOptions, logger), ICurrencyRepository
{
    public Task<List<CurrencyAggregate>> GetAllAsync(CancellationToken cancellationToken)
    {
        var filter = Builders<CurrencyAggregate>.Filter.Eq(x => x.IsActive, true);

        return GetCollection<CurrencyAggregate>().Find(filter).ToListAsync(cancellationToken);
    }
}