using System.Reflection;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;
using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Microservice.Locations.Domain;
using CodeDesignPlus.Net.Microservice.Locations.Domain.Repositories;
using CodeDesignPlus.Net.Microservice.Locations.Domain.ValueObjects;
using CodeDesignPlus.Net.Mongo.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using C = CodeDesignPlus.Net.Core.Abstractions.Models.Criteria;

namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Seeds;

/// <summary>
/// Siembra los catálogos de ubicación al arrancar, a partir de los JSON embebidos en <c>Seeds/</c>.
///
/// <para><b>Se siembra por clave, registro a registro, y nunca se toca lo que ya existe</b> (regla 25 de
/// <c>Microservices/rules/</c>, plan 039 de pendings). Por cada catálogo se leen los ids que ya hay en Mongo y se
/// insertan solo los del JSON que faltan. Así, añadir un país, una ciudad o un barrio al JSON llega a una base que ya
/// tiene datos, y lo que un administrador corrigió desde la pantalla no se pisa en el siguiente arranque.</para>
///
/// <para>Antes se comprobaba por conteo (<c>TotalCount &gt;= data.Count</c>): con un solo registro de más en Mongo, el
/// catálogo entero se daba por sembrado y lo nuevo del JSON no llegaba nunca.</para>
///
/// <para><b>Sin valores inventados.</b> Si al JSON le falta un dato obligatorio, el agregado lo rechaza y ese
/// registro se salta con un aviso en el log que dice cuál es. Antes se rellenaba con «000», «+1», «UTC», «$» o
/// «Unknown», datos falsos que nadie veía.</para>
///
/// <para>Consecuencia: <b>corregir un registro que ya está en Mongo no se hace cambiando el JSON</b>, porque la siembra
/// no actualiza. Se corrige desde la pantalla o, si es un error de la siembra en todas las bases, se borra el registro
/// y el siguiente arranque lo vuelve a crear desde el JSON.</para>
/// </summary>
public class LocationSeedService(
    ICurrencyRepository currencyRepository,
    ICountryRepository countryRepository,
    IStateRepository stateRepository,
    ICityRepository cityRepository,
    ILocalityRepository localityRepository,
    INeighborhoodRepository neighborhoodRepository,
    ITimezoneRepository timezoneRepository,
    IRegionRepository regionRepository,
    ILogger<LocationSeedService> logger
) : BackgroundService
{
    private static readonly Guid SystemUserId = Guid.Parse("10000000-0000-0000-0000-000000000001");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        try
        {
            // El orden importa: cada nivel necesita que exista su padre.
            await SeedAsync(currencyRepository, "seed-currencies.json", "currencies",
                (CurrencySeed x) => x.Id, x => x.Code,
                x => CurrencyAggregate.Create(x.Id, x.Code, x.NumericCode, x.DecimalDigits, x.Symbol, x.Name, SystemUserId), stoppingToken);

            await SeedAsync(regionRepository, "seed-regions.json", "regions",
                (RegionSeed x) => x.Id, x => x.Name,
                x => RegionAggregate.Create(x.Id, x.Name, x.SubRegions, isActive: true, SystemUserId), stoppingToken);

            await SeedAsync(countryRepository, "seed-countries.json", "countries",
                (CountrySeed x) => x.Id, x => $"{x.Name} ({x.Alpha2})",
                x => CountryAggregate.Create(x.Id, x.Name, x.Alpha2, x.Alpha3, x.Code, x.PhoneCode, x.Capital, x.IdCurrency, x.Timezone, x.NameNative!, x.Region!, x.SubRegion!, x.Latitude, x.Longitude, x.Flag, true, SystemUserId), stoppingToken);

            await SeedAsync(stateRepository, "seed-co-states.json", "states",
                (StateSeed x) => x.Id, x => x.Name,
                x => StateAggregate.Create(x.Id, x.IdCountry, x.Code, x.Name, SystemUserId), stoppingToken);

            await SeedAsync(cityRepository, "seed-co-cities.json", "cities",
                (CitySeed x) => x.Id, x => x.Name,
                x => CityAggregate.Create(x.Id, x.IdState, x.Name, x.Timezone, SystemUserId), stoppingToken);

            await SeedAsync(localityRepository, "seed-co-localities.json", "localities",
                (LocalitySeed x) => x.Id, x => x.Name,
                x => LocalityAggregate.Create(x.Id, x.IdCity, x.Name, SystemUserId), stoppingToken);

            await SeedAsync(neighborhoodRepository, "seed-co-neighborhoods.json", "neighborhoods",
                (NeighborhoodSeed x) => x.Id, x => x.Name,
                x => NeighborhoodAggregate.Create(x.Id, x.IdLocality, x.Name, SystemUserId), stoppingToken);

            await SeedAsync(timezoneRepository, "seed-timezones.json", "timezones",
                (TimezoneSeed x) => x.Id, x => x.Name,
                x => TimezoneAggregate.Create(x.Id, x.Name, x.Aliases,
                    Location.Create(x.Location.CountryCode, x.Location.CountryName, x.Location.Latitude, x.Location.Longitude),
                    x.Offsets, x.CurrentOffset, isActive: true, SystemUserId), stoppingToken);

            logger.LogInformation("Location seed completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding location data.");
        }
    }

    /// <summary>
    /// Inserta los registros del JSON cuyo id todavía no está en Mongo. Lo que ya existe no se lee ni se toca.
    /// </summary>
    private async Task SeedAsync<TSeed, TAggregate>(
        IRepositoryBase repository,
        string file,
        string catalog,
        Func<TSeed, Guid> id,
        Func<TSeed, string> label,
        Func<TSeed, TAggregate> create,
        CancellationToken ct)
        where TAggregate : class, IEntityBase
    {
        var data = LoadResource<List<TSeed>>(file);

        var existing = await repository.MatchingAsync<TAggregate, Guid>(new C.Criteria(), x => x.Id, ct);
        var ids = existing.Data.ToHashSet();

        var missing = data.Where(x => !ids.Contains(id(x))).ToList();

        if (missing.Count == 0)
        {
            logger.LogInformation("Seed of {Catalog}: nothing missing ({Existing} in Mongo, {Total} in {File}).", catalog, ids.Count, data.Count, file);
            return;
        }

        var inserted = 0;

        foreach (var item in missing)
        {
            try
            {
                await repository.CreateAsync(create(item), ct);
                inserted++;
            }
            catch (Exception ex)
            {
                // Un dato obligatorio que falta en el JSON: se salta el registro y se dice cuál, sin inventar el valor.
                logger.LogWarning(ex, "Seed of {Catalog}: skipped {Label} ({Id}) because it is invalid.", catalog, label(item), id(item));
            }
        }

        logger.LogInformation("Seed of {Catalog}: inserted {Inserted} of {Missing} missing ({Total} in {File}).", catalog, inserted, missing.Count, data.Count, file);
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
public record RegionSeed(Guid Id, string Name, List<string> SubRegions);
public record CountrySeed(Guid Id, string Name, string Alpha2, string Alpha3, string Code, string PhoneCode, string? Capital, Guid IdCurrency, string Timezone, string? NameNative, string? Region, string? SubRegion, double Latitude, double Longitude, string? Flag);
public record StateSeed(Guid Id, Guid IdCountry, string Code, string Name);
public record CitySeed(Guid Id, Guid IdState, string Name, string Timezone);
public record LocalitySeed(Guid Id, Guid IdCity, string Name);
public record NeighborhoodSeed(Guid Id, Guid IdLocality, string Name);
public record TimezoneSeed(Guid Id, string Name, List<string> Aliases, LocationSeed Location, List<string> Offsets, string CurrentOffset);
public record LocationSeed(string CountryCode, string CountryName, double Latitude, double Longitude);
