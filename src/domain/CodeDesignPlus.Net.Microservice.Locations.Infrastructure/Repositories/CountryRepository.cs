namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Repositories;

public class CountryRepository(IServiceProvider serviceProvider, IOptions<MongoOptions> mongoOptions, ILogger<CountryRepository> logger) 
    : RepositoryBase(serviceProvider, mongoOptions, logger), ICountryRepository
{
   
}