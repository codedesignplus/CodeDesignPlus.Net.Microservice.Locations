namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class CurrencyRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<CurrencyRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ICurrencyRepository
{
   
}