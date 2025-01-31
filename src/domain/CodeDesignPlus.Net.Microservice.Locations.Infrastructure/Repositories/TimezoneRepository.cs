namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class TimezoneRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<TimezoneRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ITimezoneRepository
{
   
}