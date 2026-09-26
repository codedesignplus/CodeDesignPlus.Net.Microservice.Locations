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

    public static readonly Error CurrencyCodeFormatInvalid = new("225");

    /// <summary>El indicativo telefónico debe seguir el formato +XXX, por ejemplo +57, +1 o +44.</summary>
    public static readonly Error PhoneCodeFormatIsInvalid = new("230");

    /// <summary>Borrar un padre con hijos los dejaba huérfanos (plan 043 de pendings).</summary>
    public static readonly Error CountryHasStates = new("231");
    public static readonly Error StateHasCities = new("232");
    public static readonly Error CityHasLocalities = new("233");
    public static readonly Error LocalityHasNeighborhoods = new("234");
    public static readonly Error CurrencyIsInUse = new("235");
    public static readonly Error TimezoneIsInUse = new("236");
    public static readonly Error RegionIsInUse = new("237");
}
