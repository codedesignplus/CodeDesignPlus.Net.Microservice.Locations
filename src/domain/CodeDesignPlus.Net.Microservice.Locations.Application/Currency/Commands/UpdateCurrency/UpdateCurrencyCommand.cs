namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;

[DtoGenerator]
public record UpdateCurrencyCommand(Guid Id, string Name, string Code, short NumericCode, short DecimalDigits, string Symbol, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateCurrencyCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(Errors.IdIsRequired);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Errors.CurrencyNameIsRequired)
            .MaximumLength(100).WithMessage(Errors.CurrencyNameMaxLengthExceeded);

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(Errors.CurrencyCodeIsRequired)
            .Length(3).WithMessage(Errors.CurrencyCodeLengthInvalid)
            .Matches(@"^[A-Z]{3}$").WithMessage(Errors.CurrencyCodeFormatInvalid);

        RuleFor(x => x.NumericCode)
            .InclusiveBetween((short)1, (short)999)
            .WithMessage(Errors.CurrencyNumericCodeInvalid);

        RuleFor(x => x.DecimalDigits)
            .InclusiveBetween((short)0, (short)4)
            .WithMessage(Errors.CurrencyDecimalDigitsInvalid);

        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage(Errors.CurrencySymbolIsRequired)
            .MaximumLength(10).WithMessage(Errors.CurrencySymbolMaxLengthExceeded);
    }
}
