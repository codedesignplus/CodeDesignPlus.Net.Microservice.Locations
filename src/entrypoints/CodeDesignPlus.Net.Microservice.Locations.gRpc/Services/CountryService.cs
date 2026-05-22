using CodeDesignPlus.Net.Exceptions.Guards;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindCurrencyById;
using CodeDesignPlus.Net.Microservice.Locations.Infrastructure;

namespace CodeDesignPlus.Net.Microservice.Locations.gRpc.Services;

public class CountryService(IMediator mediator, IMapper mapper) : gRpc.CountryService.CountryServiceBase
{
    public override async Task<GetCountryResponse> GetCountry(GetCountryRequest request, ServerCallContext context)
    {
        // Build a filter expression based on the provided request fields
        // Using Filters bypasses the shared cache and queries MongoDB directly
        var filter = BuildFilter(request);

        var countries = await mediator.Send(new GetAllCountryQuery(new C.Criteria
        {
            Filters = filter,
            Limit = 1
        }));

        var country = countries.Data.FirstOrDefault();

        InfrastructureGuard.IsNull(country, Errors.CountryNotFound);

        var currency = await mediator.Send(new FindCurrencyByIdQuery(country!.IdCurrency));

        InfrastructureGuard.IsNull(currency, Errors.CurrencyNotFound);

        var response = mapper.Map<GetCountryResponse>(country);
        response.Currency = mapper.Map<Currency>(currency);

        return response;
    }

    private static string BuildFilter(GetCountryRequest request)
    {
        if (!string.IsNullOrEmpty(request.Id) && Guid.TryParse(request.Id, out _))
            return $"Id={request.Id}";

        if (!string.IsNullOrEmpty(request.Alpha2))
            return $"Alpha2={request.Alpha2}";

        if (!string.IsNullOrEmpty(request.Alpha3))
            return $"Alpha3={request.Alpha3}";

        if (!string.IsNullOrEmpty(request.Code))
            return $"Code={request.Code}";

        if (!string.IsNullOrEmpty(request.Name))
            return $"Name={request.Name}";

        return "IsActive=true";
    }
}