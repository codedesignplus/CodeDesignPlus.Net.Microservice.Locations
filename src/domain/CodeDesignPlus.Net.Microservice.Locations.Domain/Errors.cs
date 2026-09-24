using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("100", "UnknownError");

    public static readonly Error IdIsInvalid = new("101", "The Id is invalid");
    public static readonly Error NameIsInvalid = new("102", "The Name is invalid"); 
    public static readonly Error CreatedByIsInvalid = new("103", "The CreatedBy is invalid"); 
    public static readonly Error UpdateByIsInvalid = new("104", "The UpdateBy is invalid"); 
    public static readonly Error DeleteByIsInvalid = new("105", "The DeleteBy is invalid"); 
    public static readonly Error IdStateIsInvalid = new("106", "The IdState is invalid");
    public static readonly Error TimezoneIsInvalid = new("107", "The Timezone is invalid"); 
    public static readonly Error CountryAlpha2IsInvalid = new("108", "The CountryAlpha2 is invalid");
    public static readonly Error IdCurrencyIsInvalid = new("109", "The IdCurrency is invalid"); 
    public static readonly Error CurrencyCodeIsInvalid = new("110", "The CurrencyCode is invalid"); 
    public static readonly Error SymbolIsInvalid = new("111", "The Symbol is invalid"); 
    public static readonly Error IdCityIsInvalid = new("112", "The IdCity is invalid"); 
    public static readonly Error IdLocalityIsInvalid = new("113", "The IdLocality is invalid"); 
    public static readonly Error IdCountryIsInvalid = new("114", "The IdCountry is invalid"); 
    public static readonly Error StateCodeIsInvalid = new("115", "The StateCode is invalid"); 
    public static readonly Error CodeRangeIsInvalid = new("116", "The Code is invalid");
    public static readonly Error Alpha3IsInvalid = new("117", "The Alpha3 is invalid");

    public static readonly Error CountryCodeIsRequired = new("118", "The CountryCode is required");
    public static readonly Error CountryNameIsRequired = new("119", "The CountryName is required");
    public static readonly Error CurrentOffsetIsInvalid = new("120", "The CurrentOffset is invalid");

    public static readonly Error LocationIsInvalid = new("122", "The Location is invalid"); 
    public static readonly Error OffsetsCanNotBeEmpty = new("123", "The Offsets can not be empty");

    public static readonly Error NameNativeIsInvalid = new("124", "The NameNative is invalid"); 
    public static readonly Error RegionIsInvalid = new("125", "The Region is invalid"); 
    public static readonly Error SubRegionIsInvalid = new("126", "The SubRegion is invalid"); 
    public static readonly Error LatitudeIsInvalid = new("127", "The Latitude is invalid"); 
    public static readonly Error LongitudeIsInvalid = new("128", "The Longitude is invalid");

    public static readonly Error NumericCodeIsInvalid = new("129", "The NumericCode is invalid");

    public static readonly Error RegionNameIsRequired = new("130", "The Region Name is required");
    public static readonly Error SubRegionsAreRequired = new("131", "The SubRegions are required");

    public static readonly Error PhoneCodeIsInvalid = new("132", "The PhoneCode is required"); 
}
