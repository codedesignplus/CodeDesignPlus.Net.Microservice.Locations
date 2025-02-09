namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindCurrencyById;

public record FindCurrencyByIdQuery(Guid Id) : IRequest<CurrencyDto>;

public class Validator : AbstractValidator<FindCurrencyByIdQuery>
{
    public Validator()
    {
        this.RuleFor(x => x.Id).NotEmpty();
    }
}