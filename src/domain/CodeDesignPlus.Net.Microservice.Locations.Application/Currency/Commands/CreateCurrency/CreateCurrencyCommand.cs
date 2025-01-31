namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;

[DtoGenerator]
public record CreateCurrencyCommand(Guid Id, string Name, string Code, string Symbol) : IRequest<Guid>;

public class Validator : AbstractValidator<CreateCurrencyCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(3);
    }
}
