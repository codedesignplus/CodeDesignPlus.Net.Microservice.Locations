namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure;

public class Errors: IErrorCodes
{    
    public const string UnknownError = "300 : UnknownError";

    public const string CountryNotFound = "301 : CountryNotFound";

    public const string CurrencyNotFound = "302 : CurrencyNotFound";
}
