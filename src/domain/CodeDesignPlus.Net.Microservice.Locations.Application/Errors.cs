using CodeDesignPlus.Net.Exceptions;

using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;

namespace CodeDesignPlus.Net.Microservice.Locations.Application;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("200");
    public static readonly Error InvalidRequest = new("201");
    public static readonly Error TimezoneAlreadyExists = new("202");
    public static readonly Error TimezoneNotFound = new("203");
    public static readonly Error CityNotFound = new("204");
    public static readonly Error CountryNotFound = new("205");
    public static readonly Error CurrencyAlreadyExists = new("206");
    public static readonly Error CurrencyNotFound = new("207");
    public static readonly Error LocalityNotFound = new("208");
    public static readonly Error NeighborhoodNotFound = new("209");
    public static readonly Error NeighborhoodAlreadyExists = new("210");
    public static readonly Error LocalityAlreadyExists = new("211");
    public static readonly Error CountryAlreadyExists = new("212");
    public static readonly Error CityAlreadyExists = new("213");
    public static readonly Error StateAlreadyExists = new("214");
    public static readonly Error StateNotFound = new("215");
    public static readonly Error RegionAlreadyExists = new("216");
    public static readonly Error RegionNotFound = new("217");

    public static readonly Error IdIsRequired = new("220");
    public static readonly Error CurrencyNameIsRequired = new("221");
    public static readonly Error CurrencyNameMaxLengthExceeded = new("222");
    public static readonly Error CurrencyCodeIsRequired = new("223");
    public static readonly Error CurrencyCodeLengthInvalid = new("224");
    public static readonly Error CurrencyCodeFormatInvalid = new("225");
    public static readonly Error CurrencyNumericCodeInvalid = new("226");
    public static readonly Error CurrencyDecimalDigitsInvalid = new("227");
    public static readonly Error CurrencySymbolIsRequired = new("228");
    public static readonly Error CurrencySymbolMaxLengthExceeded = new("229");
}
