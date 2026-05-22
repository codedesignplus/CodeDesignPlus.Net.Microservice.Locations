using CodeDesignPlus.Net.Exceptions.Guards;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindCurrencyById;
using CodeDesignPlus.Net.Microservice.Locations.Infrastructure;

namespace CodeDesignPlus.Net.Microservice.Locations.gRpc.Services;

public class CountryService(IMediator mediator, IMapper mapper) : gRpc.CountryService.CountryServiceBase
{
    public override async Task<GetCountryResponse> GetCountry(GetCountryRequest request, ServerCallContext context)
    {
        var countries = await mediator.Send(new GetAllCountryQuery(new C.Criteria
        {
            Limit = 500
        }));

        var countryId = Guid.TryParse(request.Id, out var parsedId) ? parsedId : Guid.Empty;

        var country = countries.Data.FirstOrDefault(x =>
            (countryId != Guid.Empty && x.Id == countryId) ||
            (!string.IsNullOrEmpty(request.Name) && x.Name == request.Name) ||
            (!string.IsNullOrEmpty(request.Code) && x.Code == request.Code) ||
            (!string.IsNullOrEmpty(request.Alpha2) && x.Alpha2 == request.Alpha2) ||
            (!string.IsNullOrEmpty(request.Alpha3) && x.Alpha3 == request.Alpha3)
        );

        InfrastructureGuard.IsNull(country, Errors.CountryNotFound);

        var currency = await mediator.Send(new FindCurrencyByIdQuery(country.IdCurrency));

        InfrastructureGuard.IsNull(currency, Errors.CurrencyNotFound);

        var response = mapper.Map<GetCountryResponse>(country);
        response.Currency = mapper.Map<Currency>(currency);

        return response;
    }
}