using CodeDesignPlus.Net.Exceptions.Guards;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.GetAllCurrencies;
using CodeDesignPlus.Net.Microservice.Locations.Infrastructure;

namespace CodeDesignPlus.Net.Microservice.Locations.gRpc.Services;

/// <summary>
/// La moneda que piden los demás micros para convertir importes a la unidad mínima (sus dígitos decimales).
///
/// Consulta con filtro, y por tanto contra Mongo, igual que <see cref="CountryService"/>. Antes pedía la lista completa
/// sin filtro, que sale de una caché de 6 horas: una moneda nueva no existía para nadie durante ese tiempo y un cambio
/// de dígitos decimales tardaba hasta 6 horas en llegar a la conversión (plan 037 de pendings).
/// </summary>
public class CurrencyService(IMediator mediator, IMapper mapper) : gRpc.CurrencyService.CurrencyServiceBase
{
    public override async Task<GetCurrencyResponse> GetCurrency(GetCurrencyRequest request, ServerCallContext context)
    {
        var currencies = await mediator.Send(new GetAllCurrenciesQuery(new C.Criteria
        {
            Filters = BuildFilter(request),
            Limit = 1
        }));

        var currency = currencies.Data.FirstOrDefault();

        InfrastructureGuard.IsNull(currency, Errors.CurrencyNotFound);

        return mapper.Map<GetCurrencyResponse>(currency);
    }

    private static string BuildFilter(GetCurrencyRequest request)
    {
        if (!string.IsNullOrEmpty(request.Id) && Guid.TryParse(request.Id, out _))
            return $"Id={request.Id}";

        if (!string.IsNullOrEmpty(request.Code))
            return $"Code={request.Code}";

        if (request.NumericCode != 0)
            return $"NumericCode={request.NumericCode}";

        if (!string.IsNullOrEmpty(request.Name))
            return $"Name={request.Name}";

        // Sin ningun criterio no hay moneda que devolver: un filtro que no casa con nada acaba en CurrencyNotFound,
        // en vez de devolver la primera moneda de la coleccion como hacia la busqueda en memoria.
        return $"Id={Guid.Empty}";
    }
}
