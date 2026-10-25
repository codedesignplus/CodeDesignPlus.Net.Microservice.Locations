namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class CurrencyRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<CurrencyRepository> logger)
    : RepositoryBase(serviceProvider, mongoOptions, logger), ICurrencyRepository
{
    public Task<List<CurrencyAggregate>> GetAllAsync(CancellationToken cancellationToken)
    {
        var filter = Builders<CurrencyAggregate>.Filter.Eq(x => x.IsActive, true);

        return GetCollection<CurrencyAggregate>().Find(filter).ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> CreateIfAbsentAsync(CurrencyAggregate currency, CancellationToken cancellationToken)
    {
        try
        {
            await CreateAsync(currency, cancellationToken);

            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}