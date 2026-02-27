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

        var countryId = Guid.Parse(request.Id ?? Guid.Empty.ToString());

        var country = countries.Data.FirstOrDefault(x =>
            x.Id == countryId ||
            x.Name == request.Name ||
            x.Code == request.Code ||
            x.Alpha2 == request.Alpha2 ||
            x.Alpha3 == request.Alpha3
        );

        InfrastructureGuard.IsNull(country, Errors.CountryNotFound);

        var currency = await mediator.Send(new FindCurrencyByIdQuery(country.IdCurrency));

        InfrastructureGuard.IsNull(currency, Errors.CurrencyNotFound);

        var response = mapper.Map<GetCountryResponse>(country);
        response.Currency = mapper.Map<Currency>(currency);

        return response;
    }
}