// This is a C# script (.csx) to generate seed JSON files for ms-locations
// Run with: dotnet script generate-seeds.csx
// Or: dotnet run if wrapped in a console app

using System.Text.Json;
using System.Text.Json.Serialization;

var options = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
var seedsPath = Path.Combine("..", "src", "domain", "CodeDesignPlus.Net.Microservice.Locations.Infrastructure", "Seeds");
Directory.CreateDirectory(seedsPath);

// ============ CURRENCIES ============
var copId = Guid.Parse("a1b2c3d4-0001-4000-8000-000000000001");
var usdId = Guid.Parse("a1b2c3d4-0001-4000-8000-000000000002");
var eurId = Guid.Parse("a1b2c3d4-0001-4000-8000-000000000003");

var currencies = new List<object>
{
    new { id = copId, code = "COP", numericCode = (short)170, decimalDigits = (short)2, symbol = "$", name = "Peso Colombiano" },
    new { id = usdId, code = "USD", numericCode = (short)840, decimalDigits = (short)2, symbol = "$", name = "US Dollar" },
    new { id = eurId, code = "EUR", numericCode = (short)978, decimalDigits = (short)2, symbol = "€", name = "Euro" },
    // ... add more
};

File.WriteAllText(Path.Combine(seedsPath, "seed-currencies.json"), JsonSerializer.Serialize(currencies, options));
Console.WriteLine($"Generated {currencies.Count} currencies");

// Generate countries, states, cities, localities, neighborhoods similarly...
Console.WriteLine("Done. Check the Seeds folder.");
