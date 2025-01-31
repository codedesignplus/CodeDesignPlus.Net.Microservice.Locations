namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class Errors: IErrorCodes
{    
    public const string UnknownError = "100 : UnknownError";

    public static string IdIsInvalid { get; internal set; }
    public static string NameIsInvalid { get; internal set; }
    public static string CreatedByIsInvalid { get; internal set; }
    public static string UpdateByIsInvalid { get; internal set; }
    public static string DeleteByIsInvalid { get; internal set; }
}
