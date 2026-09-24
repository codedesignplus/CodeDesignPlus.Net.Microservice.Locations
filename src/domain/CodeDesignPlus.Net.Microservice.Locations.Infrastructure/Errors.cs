using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Locations.Infrastructure;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("300");

    public static readonly Error CountryNotFound = new("301");

    public static readonly Error CurrencyNotFound = new("302");
}
