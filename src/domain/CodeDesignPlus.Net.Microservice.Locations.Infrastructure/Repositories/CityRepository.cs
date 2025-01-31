namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class CityRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<CityRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ICityRepository
{
   
}