namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class NeighborhoodRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<NeighborhoodRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), INeighborhoodRepository
{
   
}