namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;

[DtoGenerator]
public record CreateCurrencyCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<CreateCurrencyCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
