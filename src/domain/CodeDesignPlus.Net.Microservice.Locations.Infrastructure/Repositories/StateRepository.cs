namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class StateRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<StateRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IStateRepository
{

    public Task<bool> AnyByCountryAsync(Guid idCountry, CancellationToken cancellationToken)
        => GetCollection<StateAggregate>().Find(x => x.IdCountry == idCountry).AnyAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<bool> CreateIfAbsentAsync(StateAggregate state, CancellationToken cancellationToken)
    {
        try
        {
            await CreateAsync(state, cancellationToken);

            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}
