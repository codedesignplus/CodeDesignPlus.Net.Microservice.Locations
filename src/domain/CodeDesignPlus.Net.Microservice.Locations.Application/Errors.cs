using CodeDesignPlus.Net.Exceptions;

using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;

namespace CodeDesignPlus.Net.Microservice.Locations.Application;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("200", "UnknownError");
    public static readonly Error InvalidRequest = new("201", "The request is invalid.");
    public static readonly Error TimezoneAlreadyExists = new("202", "The timezone already exists.");
    public static readonly Error TimezoneNotFound = new("203", "The timezone was not found.");
    public static readonly Error CityNotFound = new("204", "The city was not found.");
    public static readonly Error CountryNotFound = new("205", "The country was not found.");
    public static readonly Error CurrencyAlreadyExists = new("206", "The currency already exists.");
    public static readonly Error CurrencyNotFound = new("207", "The currency was not found.");
    public static readonly Error LocalityNotFound = new("208", "The locality was not found.");
    public static readonly Error NeighborhoodNotFound = new("209", "The neighborhood was not found.");
    public static readonly Error NeighborhoodAlreadyExists = new("210", "The neighborhood already exists.");
    public static readonly Error LocalityAlreadyExists = new("211", "The locality already exists.");
    public static readonly Error CountryAlreadyExists = new("212", "The country already exists.");
    public static readonly Error CityAlreadyExists = new("213", "The city already exists.");
    public static readonly Error StateAlreadyExists = new("214", "The state already exists.");
    public static readonly Error StateNotFound = new("215", "The state was not found.");
    public static readonly Error RegionAlreadyExists = new("216", "The region already exists.");
    public static readonly Error RegionNotFound = new("217", "The region was not found.");

    public static readonly Error IdIsRequired = new("220", "Id is required.");
    public static readonly Error CurrencyNameIsRequired = new("221", "Currency name is required.");
    public static readonly Error CurrencyNameMaxLengthExceeded = new("222", "Currency name must not exceed 100 characters.");
    public static readonly Error CurrencyCodeIsRequired = new("223", "ISO Alpha code is required.");
    public static readonly Error CurrencyCodeLengthInvalid = new("224", "ISO Alpha code must be exactly 3 uppercase letters.");
    public static readonly Error CurrencyCodeFormatInvalid = new("225", "ISO Alpha code must consist of only uppercase letters (A-Z).");
    public static readonly Error CurrencyNumericCodeInvalid = new("226", "ISO Numeric code must be between 1 and 999.");
    public static readonly Error CurrencyDecimalDigitsInvalid = new("227", "Decimal digits must be between 0 and 4.");
    public static readonly Error CurrencySymbolIsRequired = new("228", "Currency symbol is required.");
    public static readonly Error CurrencySymbolMaxLengthExceeded = new("229", "Currency symbol must not exceed 10 characters.");
}
