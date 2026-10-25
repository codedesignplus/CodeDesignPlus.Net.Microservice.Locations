namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class RegionRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RegionRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IRegionRepository
{
    /// <inheritdoc />
    public async Task<bool> CreateIfAbsentAsync(RegionAggregate region, CancellationToken cancellationToken)
    {
        try
        {
            await CreateAsync(region, cancellationToken);

            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}
