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

        var currencyId = Guid.Parse(request.Id ?? Guid.Empty.ToString());

        var currency = currencies.Data.FirstOrDefault(x =>
            x.Id == currencyId ||
            x.Name == request.Name ||
            x.Code == request.Code ||
            x.NumericCode == request.NumericCode
        );

        InfrastructureGuard.IsNull(currency, Errors.CurrencyNotFound);

        return mapper.Map<GetCurrencyResponse>(currency);
    }
}