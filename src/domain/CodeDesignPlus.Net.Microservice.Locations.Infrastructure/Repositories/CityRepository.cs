namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class CityRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<CityRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ICityRepository
{

    public Task<bool> AnyByStateAsync(Guid idState, CancellationToken cancellationToken)
        => GetCollection<CityAggregate>().Find(x => x.IdState == idState).AnyAsync(cancellationToken);

    public Task<bool> AnyByTimezoneAsync(string timezone, CancellationToken cancellationToken)
        => GetCollection<CityAggregate>().Find(x => x.Timezone == timezone).AnyAsync(cancellationToken);
}
