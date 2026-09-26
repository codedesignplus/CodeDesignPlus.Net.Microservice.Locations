namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class StateRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<StateRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IStateRepository
{

    public Task<bool> AnyByCountryAsync(Guid idCountry, CancellationToken cancellationToken)
        => GetCollection<StateAggregate>().Find(x => x.IdCountry == idCountry).AnyAsync(cancellationToken);
}
