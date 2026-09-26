namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class LocalityRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<LocalityRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ILocalityRepository
{

    public Task<bool> AnyByCityAsync(Guid idCity, CancellationToken cancellationToken)
        => GetCollection<LocalityAggregate>().Find(x => x.IdCity == idCity).AnyAsync(cancellationToken);
}
