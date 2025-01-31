namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindCurrencyById;

public record FindCurrencyByIdQuery(Guid Id) : IRequest<CurrencyDto>;

