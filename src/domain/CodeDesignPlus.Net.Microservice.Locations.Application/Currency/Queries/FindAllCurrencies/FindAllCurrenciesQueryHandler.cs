namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindAllCurrencies;

public class FindAllCurrenciesQueryHandler(ICurrencyRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindAllCurrenciesQuery, CurrencyDto>
{
    public Task<CurrencyDto> Handle(FindAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<CurrencyDto>(default!);
    }
}
