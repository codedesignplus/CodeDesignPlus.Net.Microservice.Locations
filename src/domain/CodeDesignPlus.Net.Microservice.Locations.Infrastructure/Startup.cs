using CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Seeds;

namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure
{
    public class Startup : IStartup
    {
        public void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<LocationSeedService>();
        }
    }
}
