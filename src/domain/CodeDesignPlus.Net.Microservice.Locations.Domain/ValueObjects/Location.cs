using System.Text.Json.Serialization;

namespace CodeDesignPlus.Net.Microservice.Locations.Domain.ValueObjects;

public sealed partial class Location
{
    public string CountryCode { get; private set; }

    public string CountryName { get; private set; }

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    [JsonConstructor]
    public Location(string countryCode, string countryName,  double latitude, double longitude)
    {
        DomainGuard.IsNullOrEmpty(countryCode, Errors.CountryCodeIsRequired);
        DomainGuard.IsNullOrEmpty(countryName, Errors.CountryNameIsRequired);

        this.CountryCode = countryCode;
        this.CountryName = countryName;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public static Location Create(string countryCode, string countryName, double latitude, double longitude)
    {
        return new Location(countryCode, countryName, latitude, longitude);
    }
}
