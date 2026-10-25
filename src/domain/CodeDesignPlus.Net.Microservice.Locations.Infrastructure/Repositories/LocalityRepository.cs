namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class LocalityRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<LocalityRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ILocalityRepository
{

    public Task<bool> AnyByCityAsync(Guid idCity, CancellationToken cancellationToken)
        => GetCollection<LocalityAggregate>().Find(x => x.IdCity == idCity).AnyAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<bool> CreateIfAbsentAsync(LocalityAggregate locality, CancellationToken cancellationToken)
    {
        try
        {
            await CreateAsync(locality, cancellationToken);

            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}
