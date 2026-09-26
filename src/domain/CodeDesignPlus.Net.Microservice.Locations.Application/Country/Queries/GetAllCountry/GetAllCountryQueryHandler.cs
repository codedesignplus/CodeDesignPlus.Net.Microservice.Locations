using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;

/// <summary>
/// Respeta siempre el criterio: filtro, página, límite y orden. Sin filtro devolvía la lista entera desde una caché de
/// 6 horas, y la pantalla que pedía 10 ordenados recibía los 250 países de golpe y sin orden (plan 038 de pendings).
/// Quien necesita la lista completa (los desplegables) la pide con un límite alto. Sin caché a propósito, como
/// Regiones: son pocos documentos, y una clave por combinación de filtro, página y orden cuesta más de lo que ahorra.
/// </summary>
public class GetAllCountryQueryHandler(ICountryRepository repository, IMapper mapper) : IRequestHandler<GetAllCountryQuery, Pagination<CountryDto>>
{
    public async Task<Pagination<CountryDto>> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var result = await repository.MatchingAsync<CountryAggregate>(request.Criteria, cancellationToken);

        return mapper.Map<Pagination<CountryDto>>(result);
    }
}
