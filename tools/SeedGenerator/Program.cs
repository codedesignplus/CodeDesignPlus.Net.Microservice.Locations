using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

var seedsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "src", "domain", "CodeDesignPlus.Net.Microservice.Locations.Infrastructure", "Seeds"));
Directory.CreateDirectory(seedsPath);

Console.WriteLine($"Seeds output path: {seedsPath}");

using var http = new HttpClient();
http.DefaultRequestHeaders.Add("User-Agent", "KappaliSeedGenerator/1.0");

// ═══════════════════════════════════════════════════════════════════
// STEP 1: Fetch all countries from mledoze/countries (ISO 3166 source)
// ═══════════════════════════════════════════════════════════════════
Console.WriteLine("\n=== Fetching data from mledoze/countries (ISO 3166 + ISO 4217) ===");

var countriesRaw = await http.GetFromJsonAsync<List<RestCountry>>(
    "https://raw.githubusercontent.com/mledoze/countries/master/countries.json");

if (countriesRaw == null || countriesRaw.Count == 0)
{
    Console.WriteLine("ERROR: Could not fetch countries data. Check internet connection.");
    return;
}
Console.WriteLine($"Fetched {countriesRaw.Count} countries");

// ═══════════════════════════════════════════════════════════════════
// STEP 2: CURRENCIES (extracted from countries - ISO 4217)
// ═══════════════════════════════════════════════════════════════════
Console.WriteLine("\n=== Generating Currencies (ISO 4217) ===");

// ISO 4217 numeric codes and decimal digits
var iso4217 = new Dictionary<string, (short numeric, short decimals)>
{
    ["AED"] = (784, 2), ["AFN"] = (971, 2), ["ALL"] = (8, 2), ["AMD"] = (51, 2), ["ANG"] = (532, 2),
    ["AOA"] = (973, 2), ["ARS"] = (32, 2), ["AUD"] = (36, 2), ["AWG"] = (533, 2), ["AZN"] = (944, 2),
    ["BAM"] = (977, 2), ["BBD"] = (52, 2), ["BDT"] = (50, 2), ["BGN"] = (975, 2), ["BHD"] = (48, 3),
    ["BIF"] = (108, 0), ["BMD"] = (60, 2), ["BND"] = (96, 2), ["BOB"] = (68, 2), ["BRL"] = (986, 2),
    ["BSD"] = (44, 2), ["BTN"] = (64, 2), ["BWP"] = (72, 2), ["BYN"] = (933, 2), ["BZD"] = (84, 2),
    ["CAD"] = (124, 2), ["CDF"] = (976, 2), ["CHF"] = (756, 2), ["CLP"] = (152, 0), ["CNY"] = (156, 2),
    ["COP"] = (170, 2), ["CRC"] = (188, 2), ["CUP"] = (192, 2), ["CVE"] = (132, 2), ["CZK"] = (203, 2),
    ["DJF"] = (262, 0), ["DKK"] = (208, 2), ["DOP"] = (214, 2), ["DZD"] = (12, 2), ["EGP"] = (818, 2),
    ["ERN"] = (232, 2), ["ETB"] = (230, 2), ["EUR"] = (978, 2), ["FJD"] = (242, 2), ["FKP"] = (238, 2),
    ["GBP"] = (826, 2), ["GEL"] = (981, 2), ["GHS"] = (936, 2), ["GIP"] = (292, 2), ["GMD"] = (270, 2),
    ["GNF"] = (324, 0), ["GTQ"] = (320, 2), ["GYD"] = (328, 2), ["HKD"] = (344, 2), ["HNL"] = (340, 2),
    ["HTG"] = (332, 2), ["HUF"] = (348, 2), ["IDR"] = (360, 2), ["ILS"] = (376, 2), ["INR"] = (356, 2),
    ["IQD"] = (368, 3), ["IRR"] = (364, 2), ["ISK"] = (352, 0), ["JMD"] = (388, 2), ["JOD"] = (400, 3),
    ["JPY"] = (392, 0), ["KES"] = (404, 2), ["KGS"] = (417, 2), ["KHR"] = (116, 2), ["KMF"] = (174, 0),
    ["KPW"] = (408, 2), ["KRW"] = (410, 0), ["KWD"] = (414, 3), ["KYD"] = (136, 2), ["KZT"] = (398, 2),
    ["LAK"] = (418, 2), ["LBP"] = (422, 2), ["LKR"] = (144, 2), ["LRD"] = (430, 2), ["LSL"] = (426, 2),
    ["LYD"] = (434, 3), ["MAD"] = (504, 2), ["MDL"] = (498, 2), ["MGA"] = (969, 2), ["MKD"] = (807, 2),
    ["MMK"] = (104, 2), ["MNT"] = (496, 2), ["MOP"] = (446, 2), ["MRU"] = (929, 2), ["MUR"] = (480, 2),
    ["MVR"] = (462, 2), ["MWK"] = (454, 2), ["MXN"] = (484, 2), ["MYR"] = (458, 2), ["MZN"] = (943, 2),
    ["NAD"] = (516, 2), ["NGN"] = (566, 2), ["NIO"] = (558, 2), ["NOK"] = (578, 2), ["NPR"] = (524, 2),
    ["NZD"] = (554, 2), ["OMR"] = (512, 3), ["PAB"] = (590, 2), ["PEN"] = (604, 2), ["PGK"] = (598, 2),
    ["PHP"] = (608, 2), ["PKR"] = (586, 2), ["PLN"] = (985, 2), ["PYG"] = (600, 0), ["QAR"] = (634, 2),
    ["RON"] = (946, 2), ["RSD"] = (941, 2), ["RUB"] = (643, 2), ["RWF"] = (646, 0), ["SAR"] = (682, 2),
    ["SBD"] = (90, 2), ["SCR"] = (690, 2), ["SDG"] = (938, 2), ["SEK"] = (752, 2), ["SGD"] = (702, 2),
    ["SHP"] = (654, 2), ["SLE"] = (925, 2), ["SOS"] = (706, 2), ["SRD"] = (968, 2), ["SSP"] = (728, 2),
    ["STN"] = (930, 2), ["SYP"] = (760, 2), ["SZL"] = (748, 2), ["THB"] = (764, 2), ["TJS"] = (972, 2),
    ["TMT"] = (934, 2), ["TND"] = (788, 3), ["TOP"] = (776, 2), ["TRY"] = (949, 2), ["TTD"] = (780, 2),
    ["TWD"] = (901, 2), ["TZS"] = (834, 2), ["UAH"] = (980, 2), ["UGX"] = (800, 0), ["USD"] = (840, 2),
    ["UYU"] = (858, 2), ["UZS"] = (860, 2), ["VES"] = (928, 2), ["VND"] = (704, 0), ["VUV"] = (548, 0),
    ["WST"] = (882, 2), ["XAF"] = (950, 0), ["XCD"] = (951, 2), ["XOF"] = (952, 0), ["XPF"] = (953, 0),
    ["YER"] = (886, 2), ["ZAR"] = (710, 2), ["ZMW"] = (967, 2), ["ZWL"] = (932, 2),
    ["HRK"] = (191, 2), ["SLL"] = (694, 2), ["VEF"] = (937, 2), ["ZWD"] = (716, 2),
    ["GGP"] = (0, 2), ["IMP"] = (0, 2), ["JEP"] = (0, 2), ["TVD"] = (0, 2), ["FOK"] = (0, 2), ["KID"] = (0, 2)
};

var currencyMap = new Dictionary<string, CurrencySeed>();
foreach (var country in countriesRaw)
{
    if (country.Currencies == null) continue;
    foreach (var (code, info) in country.Currencies)
    {
        if (!currencyMap.ContainsKey(code))
        {
            var (numeric, decimals) = iso4217.GetValueOrDefault(code, ((short)0, (short)2));
            currencyMap[code] = new CurrencySeed(Guid.NewGuid(), code, numeric, decimals, info.Symbol ?? "$", info.Name ?? code);
        }
    }
}

var currencies = currencyMap.Values.OrderBy(c => c.Code).ToList();
File.WriteAllText(Path.Combine(seedsPath, "seed-currencies.json"), JsonSerializer.Serialize(currencies, options));
Console.WriteLine($"Generated {currencies.Count} currencies (with ISO 4217 numeric codes)");

// ═══════════════════════════════════════════════════════════════════
// STEP 3: COUNTRIES (ISO 3166 - all ~250)
// ═══════════════════════════════════════════════════════════════════
Console.WriteLine("\n=== Generating Countries (ISO 3166) ===");

var countries = new List<CountrySeed>();
foreach (var c in countriesRaw.OrderBy(c => c.Name?.Common))
{
    var currencyCode = c.Currencies?.Keys.FirstOrDefault();
    var currencyId = currencyCode != null && currencyMap.ContainsKey(currencyCode) ? currencyMap[currencyCode].Id : Guid.Empty;

    countries.Add(new CountrySeed(
        Guid.NewGuid(),
        c.Name?.Common ?? "Unknown",
        c.Cca2 ?? "XX",
        c.Cca3 ?? "XXX",
        c.Ccn3 ?? "000",
        c.Capital?.FirstOrDefault(),
        currencyId,
        "UTC",
        c.Translations?.ContainsKey("spa") == true ? c.Translations["spa"].Common : c.Name?.Common,
        c.Region ?? "Unknown",
        c.Subregion ?? "Unknown",
        c.Latlng?.Count >= 2 ? c.Latlng[0] : 0,
        c.Latlng?.Count >= 2 ? c.Latlng[1] : 0,
        c.Flag
    ));
}

File.WriteAllText(Path.Combine(seedsPath, "seed-countries.json"), JsonSerializer.Serialize(countries, options));
Console.WriteLine($"Generated {countries.Count} countries");

var colombiaId = countries.First(c => c.Alpha2 == "CO").Id;
Console.WriteLine($"Colombia ID: {colombiaId}");

// ═══════════════════════════════════════════════════════════════════
// STEP 4: COLOMBIA - Departamentos (DANE API)
// ═══════════════════════════════════════════════════════════════════
Console.WriteLine("\n=== Generating Colombian Departments (DANE) ===");

List<DaneMunicipio>? daneData = null;
try
{
    daneData = await http.GetFromJsonAsync<List<DaneMunicipio>>(
        "https://www.datos.gov.co/resource/gdxc-w37w.json?%24limit=1200");
    Console.WriteLine($"Fetched {daneData?.Count ?? 0} records from DANE API");
}
catch (Exception ex)
{
    Console.WriteLine($"DANE API error: {ex.Message}. Using fallback data.");
}

var states = new List<StateSeed>();
var cities = new List<CitySeed>();

if (daneData != null && daneData.Count > 0)
{
    // Extract unique departments
    var departments = daneData
        .Where(d => !string.IsNullOrEmpty(d.Departamento) && !string.IsNullOrEmpty(d.C_digo_dane_del_departamento))
        .Select(d => new { Code = d.C_digo_dane_del_departamento!, Name = d.Departamento! })
        .DistinctBy(d => d.Code)
        .OrderBy(d => d.Code)
        .ToList();

    foreach (var dept in departments)
        states.Add(new StateSeed(Guid.NewGuid(), colombiaId, dept.Code, dept.Name));

    Console.WriteLine($"Generated {states.Count} departments from DANE");

    // Extract all municipalities
    foreach (var mun in daneData.Where(m => !string.IsNullOrEmpty(m.Municipio)).OrderBy(m => m.Municipio))
    {
        var state = states.FirstOrDefault(s => s.Code == mun.C_digo_dane_del_departamento);
        if (state == null) continue;
        cities.Add(new CitySeed(Guid.NewGuid(), state.Id, mun.Municipio!, "America/Bogota"));
    }
    Console.WriteLine($"Generated {cities.Count} municipalities from DANE");
}
else
{
    // Fallback: built-in department list
    var deptData = new (string code, string name)[]
    {
        ("91", "Amazonas"), ("05", "Antioquia"), ("81", "Arauca"), ("08", "Atlantico"),
        ("11", "Bogota D.C."), ("13", "Bolivar"), ("15", "Boyaca"), ("17", "Caldas"),
        ("18", "Caqueta"), ("19", "Cauca"), ("20", "Cesar"), ("27", "Choco"),
        ("23", "Cordoba"), ("25", "Cundinamarca"), ("94", "Guainia"), ("95", "Guaviare"),
        ("41", "Huila"), ("44", "La Guajira"), ("47", "Magdalena"), ("50", "Meta"),
        ("52", "Narino"), ("54", "Norte de Santander"), ("86", "Putumayo"),
        ("63", "Quindio"), ("66", "Risaralda"), ("68", "Santander"),
        ("70", "Sucre"), ("73", "Tolima"), ("76", "Valle del Cauca"),
        ("97", "Vaupes"), ("99", "Vichada"), ("85", "Casanare"),
        ("88", "San Andres y Providencia")
    };
    foreach (var (code, name) in deptData)
        states.Add(new StateSeed(Guid.NewGuid(), colombiaId, code, name));

    // Fallback: capital cities only
    var capitalData = new (string deptCode, string city)[]
    {
        ("11", "Bogota"), ("05", "Medellin"), ("76", "Cali"), ("08", "Barranquilla"),
        ("13", "Cartagena"), ("68", "Bucaramanga"), ("50", "Villavicencio"),
        ("73", "Ibague"), ("19", "Popayan"), ("52", "Pasto"), ("47", "Santa Marta"),
        ("66", "Pereira"), ("17", "Manizales"), ("41", "Neiva"), ("63", "Armenia"),
        ("54", "Cucuta"), ("70", "Sincelejo"), ("20", "Valledupar"), ("44", "Riohacha"),
        ("23", "Monteria"), ("15", "Tunja"), ("27", "Quibdo"), ("86", "Mocoa"),
        ("18", "Florencia"), ("81", "Arauca"), ("85", "Yopal"), ("95", "San Jose del Guaviare"),
        ("94", "Inirida"), ("91", "Leticia"), ("97", "Mitu"), ("99", "Puerto Carreno"),
        ("88", "San Andres"), ("25", "Bogota")
    };
    foreach (var (deptCode, city) in capitalData)
    {
        var state = states.FirstOrDefault(s => s.Code == deptCode);
        if (state != null)
            cities.Add(new CitySeed(Guid.NewGuid(), state.Id, city, "America/Bogota"));
    }
    Console.WriteLine($"Fallback: {states.Count} departments, {cities.Count} cities");
}

File.WriteAllText(Path.Combine(seedsPath, "seed-co-states.json"), JsonSerializer.Serialize(states, options));
File.WriteAllText(Path.Combine(seedsPath, "seed-co-cities.json"), JsonSerializer.Serialize(cities, options));

// ═══════════════════════════════════════════════════════════════════
// STEP 5: Localities (all major Colombian cities)
// ═══════════════════════════════════════════════════════════════════
Console.WriteLine("\n=== Generating Localities ===");

var localities = new List<LocalitySeed>();

Guid FindCity(params string[] patterns) => cities.FirstOrDefault(c => patterns.Any(p => c.Name.Contains(p, StringComparison.OrdinalIgnoreCase)))?.Id ?? Guid.Empty;

var bogotaId = FindCity("Bogot", "BOGOTA");
var medellinId = FindCity("Medell", "MEDELLIN");
var caliId = FindCity("Cali", "SANTIAGO DE CALI");
var barranquillaId = FindCity("Barranquilla");
var cartagenaId = FindCity("Cartagena");
var bucaramangaId = FindCity("Bucaramanga");
var cucutaId = FindCity("Cucuta", "CÚCUTA", "SAN JOSE DE CUCUTA");
var pereiraId = FindCity("Pereira");
var manizalesId = FindCity("Manizales");
var ibagueId = FindCity("Ibague", "IBAGUÉ");
var villavicencioId = FindCity("Villavicencio");
var pastoId = FindCity("Pasto", "SAN JUAN DE PASTO");
var neivaId = FindCity("Neiva");
var santaMartaId = FindCity("Santa Marta");
var armeniaId = FindCity("Armenia");
var popayánId = FindCity("Popayan", "POPAYÁN");
var valleduparId = FindCity("Valledupar");
var monteriaId = FindCity("Monteria", "MONTERÍA");

// Bogota - 20 localidades
if (bogotaId != Guid.Empty)
{
    var locs = new[] { "Usaquen", "Chapinero", "Santa Fe", "San Cristobal", "Usme", "Tunjuelito", "Bosa", "Kennedy", "Fontibon", "Engativa", "Suba", "Barrios Unidos", "Teusaquillo", "Los Martires", "Antonio Narino", "Puente Aranda", "La Candelaria", "Rafael Uribe Uribe", "Ciudad Bolivar", "Sumapaz" };
    foreach (var l in locs) localities.Add(new LocalitySeed(Guid.NewGuid(), bogotaId, l));
}

// Medellin - 16 comunas + 5 corregimientos
if (medellinId != Guid.Empty)
{
    var locs = new[] { "Popular", "Santa Cruz", "Manrique", "Aranjuez", "Castilla", "Doce de Octubre", "Robledo", "Villa Hermosa", "Buenos Aires", "La Candelaria", "Laureles-Estadio", "La America", "San Javier", "El Poblado", "Guayabal", "Belen", "Palmitas", "San Cristobal", "Altavista", "San Antonio de Prado", "Santa Elena" };
    foreach (var l in locs) localities.Add(new LocalitySeed(Guid.NewGuid(), medellinId, l));
}

// Cali - 22 comunas
if (caliId != Guid.Empty)
{
    for (int i = 1; i <= 22; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), caliId, $"Comuna {i}"));
}

// Barranquilla - 5 localidades
if (barranquillaId != Guid.Empty)
{
    var locs = new[] { "Norte-Centro Historico", "Sur Occidente", "Sur Oriente", "Metropolitana", "Riomar" };
    foreach (var l in locs) localities.Add(new LocalitySeed(Guid.NewGuid(), barranquillaId, l));
}

// Cartagena - 3 localidades
if (cartagenaId != Guid.Empty)
{
    var locs = new[] { "Historica y del Caribe Norte", "De la Virgen y Turistica", "Industrial y de la Bahia" };
    foreach (var l in locs) localities.Add(new LocalitySeed(Guid.NewGuid(), cartagenaId, l));
}

// Bucaramanga - 17 comunas
if (bucaramangaId != Guid.Empty)
{
    for (int i = 1; i <= 17; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), bucaramangaId, $"Comuna {i}"));
}

// Cucuta - 10 comunas
if (cucutaId != Guid.Empty)
{
    for (int i = 1; i <= 10; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), cucutaId, $"Comuna {i}"));
}

// Pereira - 19 comunas
if (pereiraId != Guid.Empty)
{
    var locs = new[] { "Centro", "Rio Otun", "San Joaquin", "Cuba", "Del Cafe", "El Oso", "Perla del Otun", "Consota", "El Rocio", "Universidad", "Boston", "Villavicencio", "Oriente", "San Nicolas", "Villasantana", "Ferrocarril", "Olímpica", "Belmonte", "Jardín" };
    foreach (var l in locs) localities.Add(new LocalitySeed(Guid.NewGuid(), pereiraId, l));
}

// Manizales - 11 comunas
if (manizalesId != Guid.Empty)
{
    var locs = new[] { "Atardeceres", "San Jose", "Cumanday", "La Estacion", "Ciudadela del Norte", "Cerro de Oro", "Tesorito", "Palogrande", "Universitaria", "La Fuente", "La Macarena" };
    foreach (var l in locs) localities.Add(new LocalitySeed(Guid.NewGuid(), manizalesId, l));
}

// Ibague - 13 comunas
if (ibagueId != Guid.Empty)
{
    for (int i = 1; i <= 13; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), ibagueId, $"Comuna {i}"));
}

// Villavicencio - 8 comunas
if (villavicencioId != Guid.Empty)
{
    for (int i = 1; i <= 8; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), villavicencioId, $"Comuna {i}"));
}

// Pasto - 12 comunas
if (pastoId != Guid.Empty)
{
    for (int i = 1; i <= 12; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), pastoId, $"Comuna {i}"));
}

// Neiva - 10 comunas
if (neivaId != Guid.Empty)
{
    for (int i = 1; i <= 10; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), neivaId, $"Comuna {i}"));
}

// Santa Marta - 9 comunas
if (santaMartaId != Guid.Empty)
{
    for (int i = 1; i <= 9; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), santaMartaId, $"Comuna {i}"));
}

// Armenia - 10 comunas
if (armeniaId != Guid.Empty)
{
    for (int i = 1; i <= 10; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), armeniaId, $"Comuna {i}"));
}

// Popayan - 9 comunas
if (popayánId != Guid.Empty)
{
    for (int i = 1; i <= 9; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), popayánId, $"Comuna {i}"));
}

// Valledupar - 6 comunas
if (valleduparId != Guid.Empty)
{
    for (int i = 1; i <= 6; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), valleduparId, $"Comuna {i}"));
}

// Monteria - 9 comunas
if (monteriaId != Guid.Empty)
{
    for (int i = 1; i <= 9; i++) localities.Add(new LocalitySeed(Guid.NewGuid(), monteriaId, $"Comuna {i}"));
}

File.WriteAllText(Path.Combine(seedsPath, "seed-co-localities.json"), JsonSerializer.Serialize(localities, options));
Console.WriteLine($"Generated {localities.Count} localities");

// ═══════════════════════════════════════════════════════════════════
// STEP 6: Neighborhoods (Bogota - from datos abiertos GeoJSON)
// ═══════════════════════════════════════════════════════════════════
Console.WriteLine("\n=== Generating Neighborhoods (Bogota datos abiertos) ===");

var neighborhoods = new List<NeighborhoodSeed>();

// Locality code → name mapping for Bogota
var localityCodeMap = new Dictionary<string, string>
{
    ["01"] = "Usaquen", ["02"] = "Chapinero", ["03"] = "Santa Fe", ["04"] = "San Cristobal",
    ["05"] = "Usme", ["06"] = "Tunjuelito", ["07"] = "Bosa", ["08"] = "Kennedy",
    ["09"] = "Fontibon", ["10"] = "Engativa", ["11"] = "Suba", ["12"] = "Barrios Unidos",
    ["13"] = "Teusaquillo", ["14"] = "Los Martires", ["15"] = "Antonio Narino",
    ["16"] = "Puente Aranda", ["17"] = "La Candelaria", ["18"] = "Rafael Uribe Uribe",
    ["19"] = "Ciudad Bolivar"
};

try
{
    Console.WriteLine("Downloading Bogota barrios legalizados GeoJSON...");
    var geoJsonUrl = "https://datosabiertos.bogota.gov.co/dataset/36a80ef3-02c1-4a48-b3c7-c285e1de4323/resource/ba69f395-13ed-4071-802d-3808e2ba9062/download/barriolegalizado.json";
    var geoJsonStr = await http.GetStringAsync(geoJsonUrl);
    Console.WriteLine($"Downloaded {geoJsonStr.Length / 1024}KB of GeoJSON data");

    // Parse features to extract NOMBRE and CODIGO_LOCALIDAD
    using var doc = JsonDocument.Parse(geoJsonStr);
    var features = doc.RootElement.GetProperty("features");
    var barrioSet = new HashSet<string>(); // avoid duplicates

    foreach (var feature in features.EnumerateArray())
    {
        var attrs = feature.GetProperty("attributes");
        var nombre = attrs.GetProperty("NOMBRE").GetString()?.Trim();
        var codLocalidad = attrs.GetProperty("CODIGO_LOCALIDAD").GetString()?.Trim();

        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(codLocalidad)) continue;
        if (codLocalidad == "Código Localidad") continue; // skip header row

        // Map code to locality name
        if (!localityCodeMap.TryGetValue(codLocalidad, out var localityName)) continue;

        var locality = localities.FirstOrDefault(l => l.Name == localityName);
        if (locality == null) continue;

        // Normalize name for dedup
        var key = $"{codLocalidad}|{nombre.ToUpperInvariant()}";
        if (!barrioSet.Add(key)) continue;

        // Title case the name
        var formattedName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nombre.ToLower());
        neighborhoods.Add(new NeighborhoodSeed(Guid.NewGuid(), locality.Id, formattedName));
    }

    Console.WriteLine($"Parsed {neighborhoods.Count} unique barrios from GeoJSON");
}
catch (Exception ex)
{
    Console.WriteLine($"Bogota GeoJSON error: {ex.Message}");
    Console.WriteLine("Using built-in fallback for Bogota neighborhoods");

    var fallback = new Dictionary<string, string[]>
    {
        ["Usaquen"] = ["Santa Barbara", "Usaquen", "Country Club", "Cedritos", "Santa Ana", "Bella Suiza", "La Carolina", "Verbenal", "San Cristobal Norte", "Toberin", "Barrancas", "La Uribe"],
        ["Chapinero"] = ["Chapinero Alto", "Chapinero Central", "El Refugio", "Chico Norte", "Rosales", "La Cabrera", "Lago Gaitan", "Pardo Rubio", "San Isidro", "El Paraiso"],
        ["Suba"] = ["Niza", "Spring", "Alhambra", "Rincon de Suba", "Tibabuyes", "Suba Centro", "El Prado", "San Jose de Bavaria", "Casa Blanca", "Colina Campestre", "Britalia"],
        ["Kennedy"] = ["Kennedy Central", "Carvajal", "Timiza", "Castilla", "Tintal", "Americas", "Patio Bonito", "Class", "Marsella", "Mandalay", "Villa Alsacia"],
        ["Engativa"] = ["Engativa Centro", "Garces Navas", "Bolivia", "Minuto de Dios", "Boyaca Real", "Santa Helenita", "Ferias", "Normandia", "Alamos", "Villas de Granada"],
        ["Fontibon"] = ["Fontibon Centro", "Villemar", "Modelia", "Hayuelos", "Ciudad Salitre", "Capellania", "Versalles", "Atahualpa"],
        ["Bosa"] = ["Bosa Centro", "El Recreo", "San Pablo", "Piamonte", "El Porvenir", "Chicala", "San Bernardino", "Bosa Nova"],
        ["Ciudad Bolivar"] = ["Candelaria La Nueva", "El Tesoro", "Arborizadora", "San Francisco", "Lucero", "El Paraiso", "Ismael Perdomo", "Jerusalem"],
        ["Teusaquillo"] = ["Teusaquillo", "Palermo", "Galerias", "La Soledad", "Parkway", "Nicolas de Federman", "La Esmeralda", "Quinta Paredes"],
        ["Barrios Unidos"] = ["Doce de Octubre", "San Fernando", "Los Andes", "Rionegro", "Siete de Agosto", "Colombia", "Polo Club"],
    };

    foreach (var (localityName, barrios) in fallback)
    {
        var locality = localities.FirstOrDefault(l => l.Name == localityName);
        if (locality == null) continue;
        foreach (var b in barrios) neighborhoods.Add(new NeighborhoodSeed(Guid.NewGuid(), locality.Id, b));
    }
}

File.WriteAllText(Path.Combine(seedsPath, "seed-co-neighborhoods.json"), JsonSerializer.Serialize(neighborhoods, options));
Console.WriteLine($"Generated {neighborhoods.Count} neighborhoods");

// ═══════════════════════════════════════════════════════════════════
Console.WriteLine("\n╔══════════════════════════════════════╗");
Console.WriteLine("║     SEED GENERATION COMPLETE         ║");
Console.WriteLine("╠══════════════════════════════════════╣");
Console.WriteLine($"║  Currencies:     {currencies.Count,5}              ║");
Console.WriteLine($"║  Countries:      {countries.Count,5}              ║");
Console.WriteLine($"║  States/Depts:   {states.Count,5}              ║");
Console.WriteLine($"║  Cities/Munis:   {cities.Count,5}              ║");
Console.WriteLine($"║  Localities:     {localities.Count,5}              ║");
Console.WriteLine($"║  Neighborhoods:  {neighborhoods.Count,5}              ║");
Console.WriteLine("╚══════════════════════════════════════╝");
Console.WriteLine($"\nFiles: {seedsPath}");

// ═══════════════════════════════════════════════════════════════════
// Records
// ═══════════════════════════════════════════════════════════════════
record CurrencySeed(Guid Id, string Code, short NumericCode, short DecimalDigits, string Symbol, string Name);
record CountrySeed(Guid Id, string Name, string Alpha2, string Alpha3, string Code, string? Capital, Guid IdCurrency, string Timezone, string? NameNative, string? Region, string? SubRegion, double Latitude, double Longitude, string? Flag);
record StateSeed(Guid Id, Guid IdCountry, string Code, string Name);
record CitySeed(Guid Id, Guid IdState, string Name, string Timezone);
record LocalitySeed(Guid Id, Guid IdCity, string Name);
record NeighborhoodSeed(Guid Id, Guid IdLocality, string Name);

record RestCountry
{
    [JsonPropertyName("name")] public RestCountryName? Name { get; set; }
    [JsonPropertyName("cca2")] public string? Cca2 { get; set; }
    [JsonPropertyName("cca3")] public string? Cca3 { get; set; }
    [JsonPropertyName("ccn3")] public string? Ccn3 { get; set; }
    [JsonPropertyName("capital")] public List<string>? Capital { get; set; }
    [JsonPropertyName("currencies")] public Dictionary<string, RestCurrency>? Currencies { get; set; }
    [JsonPropertyName("timezones")] public List<string>? Timezones { get; set; }
    [JsonPropertyName("region")] public string? Region { get; set; }
    [JsonPropertyName("subregion")] public string? Subregion { get; set; }
    [JsonPropertyName("latlng")] public List<double>? Latlng { get; set; }
    [JsonPropertyName("flag")] public string? Flag { get; set; }
    [JsonPropertyName("translations")] public Dictionary<string, RestTranslation>? Translations { get; set; }
}
record RestCountryName { [JsonPropertyName("common")] public string? Common { get; set; } }
record RestCurrency { [JsonPropertyName("name")] public string? Name { get; set; } [JsonPropertyName("symbol")] public string? Symbol { get; set; } }
record RestTranslation { [JsonPropertyName("common")] public string? Common { get; set; } }
record DaneMunicipio
{
    [JsonPropertyName("cod_dpto")] public string? C_digo_dane_del_departamento { get; set; }
    [JsonPropertyName("dpto")] public string? Departamento { get; set; }
    [JsonPropertyName("cod_mpio")] public string? C_digo_dane_del_municipio { get; set; }
    [JsonPropertyName("nom_mpio")] public string? Municipio { get; set; }
}
record BogotaBarrio
{
    [JsonPropertyName("nombre_de_la_localidad")] public string? Nombre_de_la_localidad { get; set; }
    [JsonPropertyName("nombre_del_barrio")] public string? Nombre_del_barrio { get; set; }
}
