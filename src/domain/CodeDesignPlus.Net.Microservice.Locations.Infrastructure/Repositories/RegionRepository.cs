namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class RegionRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<RegionRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IRegionRepository
{
   
}