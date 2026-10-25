namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class TimezoneRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<TimezoneRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ITimezoneRepository
{
    /// <inheritdoc />
    public async Task<bool> CreateIfAbsentAsync(TimezoneAggregate timezone, CancellationToken cancellationToken)
    {
        try
        {
            await CreateAsync(timezone, cancellationToken);

            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}
