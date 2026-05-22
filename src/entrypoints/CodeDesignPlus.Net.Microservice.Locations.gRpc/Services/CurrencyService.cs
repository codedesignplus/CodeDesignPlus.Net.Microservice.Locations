using System;
using CodeDesignPlus.Net.Exceptions.Guards;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.GetAllCurrencies;
using CodeDesignPlus.Net.Microservice.Locations.Infrastructure;

namespace CodeDesignPlus.Net.Microservice.Locations.gRpc.Services;

public class CurrencyService(IMediator mediator, IMapper mapper) : gRpc.CurrencyService.CurrencyServiceBase
{
    public override async Task<GetCurrencyResponse> GetCurrency(GetCurrencyRequest request, ServerCallContext context)
    {
        var currencies = await mediator.Send(new GetAllCurrenciesQuery(new C.Criteria
        {
            Limit = 500
        }));

        var currencyId = Guid.TryParse(request.Id, out var parsedId) ? parsedId : Guid.Empty;

        var currency = currencies.Data.FirstOrDefault(x =>
            (currencyId != Guid.Empty && x.Id == currencyId) ||
            (!string.IsNullOrEmpty(request.Name) && x.Name == request.Name) ||
            (!string.IsNullOrEmpty(request.Code) && x.Code == request.Code) ||
            (request.NumericCode != 0 && x.NumericCode == request.NumericCode)
        );

        InfrastructureGuard.IsNull(currency, Errors.CurrencyNotFound);

        return mapper.Map<GetCurrencyResponse>(currency);
    }
}