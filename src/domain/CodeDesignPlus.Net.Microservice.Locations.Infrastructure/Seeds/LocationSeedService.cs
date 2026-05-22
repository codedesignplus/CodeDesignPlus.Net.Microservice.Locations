using System.Reflection;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;
using CodeDesignPlus.Net.Microservice.Locations.Domain;
using CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using C = CodeDesignPlus.Net.Core.Abstractions.Models.Criteria;

namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Seeds;

public class LocationSeedService(
    ICurrencyRepository currencyRepository,
    ICountryRepository countryRepository,
    IStateRepository stateRepository,
    ICityRepository cityRepository,
    ILocalityRepository localityRepository,
    INeighborhoodRepository neighborhoodRepository,
    ILogger<LocationSeedService> logger
) : BackgroundService
{
    private static readonly Guid SystemUserId = Guid.Parse("10000000-0000-0000-0000-000000000001");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        try
        {
            await SeedCurrenciesAsync(stoppingToken);
            await SeedCountriesAsync(stoppingToken);
            await SeedStatesAsync(stoppingToken);
            await SeedCitiesAsync(stoppingToken);
            await SeedLocalitiesAsync(stoppingToken);
            await SeedNeighborhoodsAsync(stoppingToken);

            logger.LogInformation("Location seed completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding location data.");
        }
    }

    private async Task SeedCurrenciesAsync(CancellationToken ct)
    {
        var criteria = new C.Criteria { Filters = "IsActive=true", Limit = 1 };
        var existing = await currencyRepository.MatchingAsync<CurrencyAggregate>(criteria, ct);
        if (existing.TotalCount > 0) { logger.LogInformation("Currencies already seeded."); return; }

        var data = LoadResource<List<CurrencySeed>>("seed-currencies.json");
        foreach (var item in data)
        {
            var aggregate = CurrencyAggregate.Create(item.Id, item.Code, item.NumericCode, item.DecimalDigits, item.Symbol, item.Name, SystemUserId);
            await currencyRepository.CreateAsync(aggregate, ct);
        }
        logger.LogInformation("Seeded {Count} currencies.", data.Count);
    }

    private async Task SeedCountriesAsync(CancellationToken ct)
    {
        var criteria = new C.Criteria { Filters = "IsActive=true", Limit = 1 };
        var existing = await countryRepository.MatchingAsync<CountryAggregate>(criteria, ct);
        if (existing.TotalCount > 0) { logger.LogInformation("Countries already seeded."); return; }

        var data = LoadResource<List<CountrySeed>>("seed-countries.json");
        foreach (var item in data)
        {
            var aggregate = CountryAggregate.Create(item.Id, item.Name, item.Alpha2, item.Alpha3, item.Code, item.Capital, item.IdCurrency, item.Timezone, item.NameNative ?? item.Name, item.Region ?? "Unknown", item.SubRegion ?? "Unknown", item.Latitude, item.Longitude, item.Flag, true, SystemUserId);
            await countryRepository.CreateAsync(aggregate, ct);
        }
        logger.LogInformation("Seeded {Count} countries.", data.Count);
    }

    private async Task SeedStatesAsync(CancellationToken ct)
    {
        var criteria = new C.Criteria { Filters = "IsActive=true", Limit = 1 };
        var existing = await stateRepository.MatchingAsync<StateAggregate>(criteria, ct);
        if (existing.TotalCount > 0) { logger.LogInformation("States already seeded."); return; }

        var data = LoadResource<List<StateSeed>>("seed-co-states.json");
        foreach (var item in data)
        {
            var aggregate = StateAggregate.Create(item.Id, item.IdCountry, item.Code, item.Name, SystemUserId);
            await stateRepository.CreateAsync(aggregate, ct);
        }
        logger.LogInformation("Seeded {Count} states.", data.Count);
    }

    private async Task SeedCitiesAsync(CancellationToken ct)
    {
        var criteria = new C.Criteria { Filters = "IsActive=true", Limit = 1 };
        var existing = await cityRepository.MatchingAsync<CityAggregate>(criteria, ct);
        if (existing.TotalCount > 0) { logger.LogInformation("Cities already seeded."); return; }

        var data = LoadResource<List<CitySeed>>("seed-co-cities.json");
        foreach (var item in data)
        {
            var aggregate = CityAggregate.Create(item.Id, item.IdState, item.Name, item.Timezone, SystemUserId);
            await cityRepository.CreateAsync(aggregate, ct);
        }
        logger.LogInformation("Seeded {Count} cities.", data.Count);
    }

    private async Task SeedLocalitiesAsync(CancellationToken ct)
    {
        var criteria = new C.Criteria { Filters = "IsActive=true", Limit = 1 };
        var existing = await localityRepository.MatchingAsync<LocalityAggregate>(criteria, ct);
        if (existing.TotalCount > 0) { logger.LogInformation("Localities already seeded."); return; }

        var data = LoadResource<List<LocalitySeed>>("seed-co-localities.json");
        foreach (var item in data)
        {
            var aggregate = LocalityAggregate.Create(item.Id, item.IdCity, item.Name, SystemUserId);
            await localityRepository.CreateAsync(aggregate, ct);
        }
        logger.LogInformation("Seeded {Count} localities.", data.Count);
    }

    private async Task SeedNeighborhoodsAsync(CancellationToken ct)
    {
        var criteria = new C.Criteria { Filters = "IsActive=true", Limit = 1 };
        var existing = await neighborhoodRepository.MatchingAsync<NeighborhoodAggregate>(criteria, ct);
        if (existing.TotalCount > 0) { logger.LogInformation("Neighborhoods already seeded."); return; }

        var data = LoadResource<List<NeighborhoodSeed>>("seed-co-neighborhoods.json");
        foreach (var item in data)
        {
            var aggregate = NeighborhoodAggregate.Create(item.Id, item.IdLocality, item.Name, SystemUserId);
            await neighborhoodRepository.CreateAsync(aggregate, ct);
        }
        logger.LogInformation("Seeded {Count} neighborhoods.", data.Count);
    }

    private static T LoadResource<T>(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames().First(n => n.EndsWith(fileName));
        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        return JsonSerializer.Deserialize<T>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}

public record CurrencySeed(Guid Id, string Code, short NumericCode, short DecimalDigits, string Symbol, string Name);
public record CountrySeed(Guid Id, string Name, string Alpha2, string Alpha3, string Code, string? Capital, Guid IdCurrency, string Timezone, string? NameNative, string? Region, string? SubRegion, double Latitude, double Longitude, string? Flag);
public record StateSeed(Guid Id, Guid IdCountry, string Code, string Name);
public record CitySeed(Guid Id, Guid IdState, string Name, string Timezone);
public record LocalitySeed(Guid Id, Guid IdCity, string Name);
public record NeighborhoodSeed(Guid Id, Guid IdLocality, string Name);
