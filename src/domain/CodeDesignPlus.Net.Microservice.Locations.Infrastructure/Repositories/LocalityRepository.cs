namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class LocalityRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<LocalityRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ILocalityRepository
{
   
}