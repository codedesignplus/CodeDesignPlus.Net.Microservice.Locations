using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Seeds;

namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure.Test.Seeds;

/// <summary>
/// Integridad de los JSON que siembra ms-locations, leídos del ensamblado tal como los carga
/// <see cref="LocationSeedService"/>.
///
/// Nacieron del plan 039 de pendings: las 22 comunas de Cali estaban colgadas de California (Santander), las de
/// Armenia de la Armenia de Antioquia, ninguna ciudad tenía zona horaria y 137 países apuntaban a una zona que no
/// estaba en el catálogo. Nada de eso fallaba al sembrar: los errores se veían semanas después, como un desplegable
/// vacío en la compra de una copropiedad. Estas pruebas lo cortan antes.
/// </summary>
public class SeedDataTest
{
    private static readonly Guid SantiagoDeCali = Guid.Parse("c333a94c-2431-43b0-b592-978b368ed1b0");
    private static readonly Guid California = Guid.Parse("87e1967e-3106-476c-8477-c8792b498507");
    private static readonly Guid ArmeniaQuindio = Guid.Parse("6754b2c2-200d-465f-8ce8-4f1e085fe372");
    private static readonly Guid ArmeniaAntioquia = Guid.Parse("9eb52561-d0b3-4710-811b-32b2331cd46e");

    private static List<T> Load<T>(string file)
    {
        var assembly = typeof(LocationSeedService).Assembly;
        var name = assembly.GetManifestResourceNames().Single(n => n.EndsWith(file));
        using var stream = assembly.GetManifestResourceStream(name)!;
        return JsonSerializer.Deserialize<List<T>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    private static readonly List<CurrencySeed> Currencies = Load<CurrencySeed>("seed-currencies.json");
    private static readonly List<RegionSeed> Regions = Load<RegionSeed>("seed-regions.json");
    private static readonly List<CountrySeed> Countries = Load<CountrySeed>("seed-countries.json");
    private static readonly List<StateSeed> States = Load<StateSeed>("seed-co-states.json");
    private static readonly List<CitySeed> Cities = Load<CitySeed>("seed-co-cities.json");
    private static readonly List<LocalitySeed> Localities = Load<LocalitySeed>("seed-co-localities.json");
    private static readonly List<NeighborhoodSeed> Neighborhoods = Load<NeighborhoodSeed>("seed-co-neighborhoods.json");
    private static readonly List<TimezoneSeed> Timezones = Load<TimezoneSeed>("seed-timezones.json");

    [Fact]
    public void Ids_AreUniqueInEachCatalog()
    {
        Assert.Equal(Currencies.Count, Currencies.Select(x => x.Id).Distinct().Count());
        Assert.Equal(Regions.Count, Regions.Select(x => x.Id).Distinct().Count());
        Assert.Equal(Countries.Count, Countries.Select(x => x.Id).Distinct().Count());
        Assert.Equal(States.Count, States.Select(x => x.Id).Distinct().Count());
        Assert.Equal(Cities.Count, Cities.Select(x => x.Id).Distinct().Count());
        Assert.Equal(Localities.Count, Localities.Select(x => x.Id).Distinct().Count());
        Assert.Equal(Neighborhoods.Count, Neighborhoods.Select(x => x.Id).Distinct().Count());
        Assert.Equal(Timezones.Count, Timezones.Select(x => x.Id).Distinct().Count());
    }

    [Fact]
    public void EveryChild_PointsToAParentThatExists()
    {
        var currencies = Currencies.Select(x => x.Id).ToHashSet();
        var countries = Countries.Select(x => x.Id).ToHashSet();
        var states = States.Select(x => x.Id).ToHashSet();
        var cities = Cities.Select(x => x.Id).ToHashSet();
        var localities = Localities.Select(x => x.Id).ToHashSet();

        Assert.Empty(Countries.Where(x => !currencies.Contains(x.IdCurrency)).Select(x => x.Alpha2));
        Assert.Empty(States.Where(x => !countries.Contains(x.IdCountry)).Select(x => x.Name));
        Assert.Empty(Cities.Where(x => !states.Contains(x.IdState)).Select(x => x.Name));
        Assert.Empty(Localities.Where(x => !cities.Contains(x.IdCity)).Select(x => x.Name));
        Assert.Empty(Neighborhoods.Where(x => !localities.Contains(x.IdLocality)).Select(x => x.Name));
    }

    [Fact]
    public void CaliAndArmeniaComunas_HangFromTheRightCity()
    {
        // Emparejar por nombre sin el departamento colgó las comunas de Cali de California y las de Armenia (Quindío)
        // de la Armenia de Antioquia.
        Assert.Empty(Localities.Where(x => x.IdCity == California || x.IdCity == ArmeniaAntioquia).Select(x => x.Name));
        Assert.Equal(22, Localities.Count(x => x.IdCity == SantiagoDeCali));
        Assert.Equal(10, Localities.Count(x => x.IdCity == ArmeniaQuindio));
    }

    [Fact]
    public void EveryTimezoneReference_ExistsInTheTimezoneCatalog()
    {
        // País y ciudad guardan la zona por NOMBRE: una zona que no está en el catálogo deja vacío su desplegable.
        var timezones = Timezones.Select(x => x.Name).ToHashSet();

        Assert.Empty(Countries.Where(x => !timezones.Contains(x.Timezone)).Select(x => $"{x.Alpha2}: {x.Timezone}"));
        Assert.Empty(Cities.Where(x => string.IsNullOrEmpty(x.Timezone) || !timezones.Contains(x.Timezone)).Select(x => x.Name));
    }

    [Fact]
    public void EveryCountryRegion_ExistsInTheRegionCatalog()
    {
        // El país guarda región y subregión como texto: tienen que coincidir con el catálogo, en español.
        var subregions = Regions.SelectMany(r => r.SubRegions.Select(s => (r.Name, s))).ToHashSet();

        Assert.Empty(Countries.Where(x => !subregions.Contains((x.Region!, x.SubRegion!))).Select(x => $"{x.Alpha2}: {x.Region} / {x.SubRegion}"));
    }

    [Fact]
    public void TextsAreInSpanish_NotTheOldEnglishSeed()
    {
        // Los nombres en inglés se veían en la compra y en la organización (plan 045).
        Assert.Contains(Countries, x => x.Alpha2 == "CO" && x.Name == "Colombia");
        Assert.Contains(Countries, x => x.Alpha2 == "US" && x.Name == "Estados Unidos");
        Assert.Contains(Currencies, x => x.Code == "COP" && x.Name == "Peso colombiano");
        Assert.DoesNotContain(Regions, x => new[] { "Americas", "Europe", "Africa", "Oceania", "Antarctic" }.Contains(x.Name));
        Assert.DoesNotContain(Countries, x => x.Region == "Unknown" || x.SubRegion == "Unknown");
        Assert.Contains(Localities, x => x.Name == "Usaquén");
    }
}
