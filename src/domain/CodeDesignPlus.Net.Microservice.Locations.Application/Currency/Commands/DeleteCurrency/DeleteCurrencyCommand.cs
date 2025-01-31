namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.DeleteCurrency;

[DtoGenerator]
public record DeleteCurrencyCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteCurrencyCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
