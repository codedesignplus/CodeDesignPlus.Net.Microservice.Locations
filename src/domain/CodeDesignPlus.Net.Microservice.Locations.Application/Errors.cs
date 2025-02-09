namespace CodeDesignPlus.Net.Microservice.Locations.Application;

public class Errors: IErrorCodes
{    
    public const string UnknownError = "200 : UnknownError";
    public const string InvalidRequest = "201 : The request is invalid.";
    public const string TimezoneAlreadyExists = "202 : The timezone already exists.";
    public const string TimezoneNotFound = "203 : The timezone was not found.";
    public const string CityNotFound = "204 : The city was not found.";
    public const string CountryNotFound = "205 : The country was not found.";
    public const string CurrencyAlreadyExists = "206 : The currency already exists.";
    public const string CurrencyNotFound = "207 : The currency was not found.";
    public const string LocalityNotFound = "208 : The locality was not found.";
    public const string NeighborhoodNotFound = "209 : The neighborhood was not found.";
    public const string NeighborhoodAlreadyExists = "210 : The neighborhood already exists.";
    public const string LocalityAlreadyExists = "211 : The locality already exists.";
    public const string CountryAlreadyExists = "212 : The country already exists.";
    public const string CityAlreadyExists = "213 : The city already exists.";
    public const string StateAlreadyExists = "214 : The state already exists.";
    public const string StateNotFound = "215 : The state was not found.";
}
