namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class CityCurrencyRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<CityCurrencyRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ICityCurrencyRepository
{
   
}