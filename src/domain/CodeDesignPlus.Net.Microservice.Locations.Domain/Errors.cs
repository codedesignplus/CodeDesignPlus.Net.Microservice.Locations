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
    public const string TimeZoneIsInvalid = "107 : The TimeZone is invalid"; 
    public const string CountryCodeIsInvalid = "108 : The CountryCode is invalid"; 
    public const string IdCurrencyIsInvalid = "109 : The IdCurrency is invalid"; 
    public const string CurrencyCodeIsInvalid = "110 : The CurrencyCode is invalid"; 
    public const string SymbolIsInvalid = "111 : The Symbol is invalid"; 
    public const string IdCityIsInvalid = "112 : The IdCity is invalid"; 
    public const string IdLocalityIsInvalid = "113 : The IdLocality is invalid"; 
    public const string IdCountryIsInvalid = "114 : The IdCountry is invalid"; 
    public const string StateCodeIsInvalid = "115 : The StateCode is invalid"; 
}
