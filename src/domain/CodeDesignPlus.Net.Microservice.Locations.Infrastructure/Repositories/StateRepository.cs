namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class StateRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<StateRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), IStateRepository
{
   
}