namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;

[DtoGenerator]
public record UpdateCurrencyCommand(Guid Id, string Name, string Code, short NumericCode, short DecimalDigits, string Symbol, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateCurrencyCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(3)
            .Matches(@"^[A-Z]{3}$").WithErrorCode(Errors.CurrencyCodeFormatInvalid.Code);

        RuleFor(x => x.NumericCode)
            .InclusiveBetween((short)1, (short)999);

        RuleFor(x => x.DecimalDigits)
            .InclusiveBetween((short)0, (short)4);

        RuleFor(x => x.Symbol)
            .NotEmpty()
            .MaximumLength(10);
    }
}
