namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class CityRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<CityRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ICityRepository
{

    public Task<bool> AnyByStateAsync(Guid idState, CancellationToken cancellationToken)
        => GetCollection<CityAggregate>().Find(x => x.IdState == idState).AnyAsync(cancellationToken);

    public Task<bool> AnyByTimezoneAsync(string timezone, CancellationToken cancellationToken)
        => GetCollection<CityAggregate>().Find(x => x.Timezone == timezone).AnyAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<bool> CreateIfAbsentAsync(CityAggregate city, CancellationToken cancellationToken)
    {
        try
        {
            await CreateAsync(city, cancellationToken);

            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}
