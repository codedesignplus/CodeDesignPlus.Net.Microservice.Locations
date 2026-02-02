using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;

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
    public const string RegionAlreadyExists = "216 : The region already exists.";
    public const string RegionNotFound = "217 : The region was not found.";

    public const string IdIsRequired = "220 : Id is required.";
    public const string CurrencyNameIsRequired = "221 : Currency name is required.";
    public const string CurrencyNameMaxLengthExceeded = "222 : Currency name must not exceed 100 characters.";
    public const string CurrencyCodeIsRequired = "223 : ISO Alpha code is required.";
    public const string CurrencyCodeLengthInvalid = "224 : ISO Alpha code must be exactly 3 uppercase letters.";
    public const string CurrencyCodeFormatInvalid = "225 : ISO Alpha code must consist of only uppercase letters (A-Z).";
    public const string CurrencyNumericCodeInvalid = "226 : ISO Numeric code must be between 1 and 999.";
    public const string CurrencyDecimalDigitsInvalid = "227 : Decimal digits must be between 0 and 4.";
    public const string CurrencySymbolIsRequired = "228 : Currency symbol is required.";
    public const string CurrencySymbolMaxLengthExceeded = "229 : Currency symbol must not exceed 10 characters.";
}
