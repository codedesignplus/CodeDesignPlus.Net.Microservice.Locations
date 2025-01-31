namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;

[DtoGenerator]
public record UpdateCurrencyCommand(Guid Id, string Name, string Code, string Symbol, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateCurrencyCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(3);
    }
}
