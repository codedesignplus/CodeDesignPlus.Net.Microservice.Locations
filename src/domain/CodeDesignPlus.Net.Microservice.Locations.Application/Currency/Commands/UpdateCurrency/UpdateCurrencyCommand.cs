namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;

[DtoGenerator]
public record UpdateCurrencyCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<UpdateCurrencyCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
