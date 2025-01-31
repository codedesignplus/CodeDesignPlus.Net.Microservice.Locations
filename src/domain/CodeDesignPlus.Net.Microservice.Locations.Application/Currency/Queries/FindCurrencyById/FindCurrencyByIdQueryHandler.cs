namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindCurrencyById;

public class FindCurrencyByIdQueryHandler(ICurrencyRepository repository, IMapper mapper, IUserContext user) : IRequestHandler<FindCurrencyByIdQuery, CurrencyDto>
{
    public Task<CurrencyDto> Handle(FindCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<CurrencyDto>(default!);
    }
}
