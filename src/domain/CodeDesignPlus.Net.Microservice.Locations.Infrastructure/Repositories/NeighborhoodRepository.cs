namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class NeighborhoodRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<NeighborhoodRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), INeighborhoodRepository
{

    public Task<bool> AnyByLocalityAsync(Guid idLocality, CancellationToken cancellationToken)
        => GetCollection<NeighborhoodAggregate>().Find(x => x.IdLocality == idLocality).AnyAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<bool> CreateIfAbsentAsync(NeighborhoodAggregate neighborhood, CancellationToken cancellationToken)
    {
        try
        {
            await CreateAsync(neighborhood, cancellationToken);

            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}
