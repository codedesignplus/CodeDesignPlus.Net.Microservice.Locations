using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("100");

    public static readonly Error IdIsInvalid = new("101");
    public static readonly Error NameIsInvalid = new("102"); 
    public static readonly Error CreatedByIsInvalid = new("103"); 
    public static readonly Error UpdateByIsInvalid = new("104"); 
    public static readonly Error DeleteByIsInvalid = new("105"); 
    public static readonly Error IdStateIsInvalid = new("106");
    public static readonly Error TimezoneIsInvalid = new("107"); 
    public static readonly Error CountryAlpha2IsInvalid = new("108");
    public static readonly Error IdCurrencyIsInvalid = new("109"); 
    public static readonly Error CurrencyCodeIsInvalid = new("110"); 
    public static readonly Error SymbolIsInvalid = new("111"); 
    public static readonly Error IdCityIsInvalid = new("112"); 
    public static readonly Error IdLocalityIsInvalid = new("113"); 
    public static readonly Error IdCountryIsInvalid = new("114"); 
    public static readonly Error StateCodeIsInvalid = new("115"); 
    public static readonly Error CodeRangeIsInvalid = new("116");
    public static readonly Error Alpha3IsInvalid = new("117");

    public static readonly Error CountryCodeIsRequired = new("118");
    public static readonly Error CountryNameIsRequired = new("119");
    public static readonly Error CurrentOffsetIsInvalid = new("120");

    public static readonly Error LocationIsInvalid = new("122"); 
    public static readonly Error OffsetsCanNotBeEmpty = new("123");

    public static readonly Error NameNativeIsInvalid = new("124"); 
    public static readonly Error RegionIsInvalid = new("125"); 
    public static readonly Error SubRegionIsInvalid = new("126"); 
    public static readonly Error LatitudeIsInvalid = new("127"); 
    public static readonly Error LongitudeIsInvalid = new("128");

    public static readonly Error NumericCodeIsInvalid = new("129");

    public static readonly Error RegionNameIsRequired = new("130");
    public static readonly Error SubRegionsAreRequired = new("131");

    public static readonly Error PhoneCodeIsInvalid = new("132"); 
}
