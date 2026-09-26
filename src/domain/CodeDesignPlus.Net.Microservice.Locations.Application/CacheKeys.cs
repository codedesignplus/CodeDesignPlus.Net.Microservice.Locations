namespace CodeDesignPlus.Net.Microservice.Locations.Application;

/// <summary>
/// Claves de la caché de ms-locations. Las usan las consultas para guardar y los comandos para borrar, y por eso
/// viven en un solo sitio: si una escritura no borra la misma clave que la consulta guardó, el cambio queda invisible
/// hasta que caduque (plan 037 de pendings). Así pasó con las monedas, que tardaban hasta 6 horas en llegar al gRPC
/// con el que los demás micros convierten importes. Los listados no se cachean: ver GetAllCountryQueryHandler.
/// </summary>
public static class CacheKeys
{
    /// <summary>Detalle de un país.</summary>
    public static string CountryById(Guid id) => $"GetCountryByIdQuery:{id}";

    /// <summary>Detalle de una moneda.</summary>
    public static string CurrencyById(Guid id) => $"FindCurrencyByIdQuery:{id}";

    /// <summary>Detalle de un estado, una ciudad, una localidad, un barrio, una región o una zona horaria.</summary>
    public static string ById(Guid id) => id.ToString();
}
