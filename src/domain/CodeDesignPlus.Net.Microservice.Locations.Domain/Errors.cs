namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class Errors: IErrorCodes
{    
    public const string UnknownError = "100 : UnknownError";

    public const string IdIsInvalid = "101 : The Id is invalid";
    public const string NameIsInvalid = "102 : The Name is invalid"; 
    public const string CreatedByIsInvalid = "103 : The CreatedBy is invalid"; 
    public const string UpdateByIsInvalid = "104 : The UpdateBy is invalid"; 
    public const string DeleteByIsInvalid = "105 : The DeleteBy is invalid"; 
    public const string IdStateIsInvalid = "106 : The IdState is invalid";
    public const string TimezoneIsInvalid = "107 : The Timezone is invalid"; 
    public const string CountryAlpha2IsInvalid = "108 : The CountryAlpha2 is invalid";
    public const string IdCurrencyIsInvalid = "109 : The IdCurrency is invalid"; 
    public const string CurrencyCodeIsInvalid = "110 : The CurrencyCode is invalid"; 
    public const string SymbolIsInvalid = "111 : The Symbol is invalid"; 
    public const string IdCityIsInvalid = "112 : The IdCity is invalid"; 
    public const string IdLocalityIsInvalid = "113 : The IdLocality is invalid"; 
    public const string IdCountryIsInvalid = "114 : The IdCountry is invalid"; 
    public const string StateCodeIsInvalid = "115 : The StateCode is invalid"; 
    public const string CodeRangeIsInvalid = "116 : The Code is invalid";
    public const string Alpha3IsInvalid = "117 : The Alpha3 is invalid";

    public const string CountryCodeIsRequired = "118 : The CountryCode is required";
    public const string CountryNameIsRequired = "119 : The CountryName is required";
    public const string CurrentOffsetIsInvalid  = "120 : The CurrentOffset is invalid";

    public const string LocationIsInvalid = "122 : The Location is invalid"; 
    public const string OffsetsCanNotBeEmpty = "123 : The Offsets can not be empty";

    public const string NameNativeIsInvalid = "124 : The NameNative is invalid"; 
    public const string RegionIsInvalid = "125 : The Region is invalid"; 
    public const string SubRegionIsInvalid = "126 : The SubRegion is invalid"; 
    public const string LatitudeIsInvalid = "127 : The Latitude is invalid"; 
    public const string LongitudeIsInvalid = "128 : The Longitude is invalid";

    public const string NumericCodeIsInvalid = "129 : The NumericCode is invalid";

    public const string RegionNameIsRequired = "130 : The Region Name is required"; 
    public const string SubRegionsAreRequired = "131 : The SubRegions are required"; 
}
