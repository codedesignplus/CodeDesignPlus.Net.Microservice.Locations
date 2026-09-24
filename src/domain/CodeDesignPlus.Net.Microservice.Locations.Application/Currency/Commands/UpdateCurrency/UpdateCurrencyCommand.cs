namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;

[DtoGenerator]
public record UpdateCurrencyCommand(Guid Id, string Name, string Code, short NumericCode, short DecimalDigits, string Symbol, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateCurrencyCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(_ => Errors.IdIsRequired.GetMessage());

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(_ => Errors.CurrencyNameIsRequired.GetMessage())
            .MaximumLength(100).WithMessage(_ => Errors.CurrencyNameMaxLengthExceeded.GetMessage());

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(_ => Errors.CurrencyCodeIsRequired.GetMessage())
            .Length(3).WithMessage(_ => Errors.CurrencyCodeLengthInvalid.GetMessage())
            .Matches(@"^[A-Z]{3}$").WithMessage(_ => Errors.CurrencyCodeFormatInvalid.GetMessage());

        RuleFor(x => x.NumericCode)
            .InclusiveBetween((short)1, (short)999)
            .WithMessage(_ => Errors.CurrencyNumericCodeInvalid.GetMessage());

        RuleFor(x => x.DecimalDigits)
            .InclusiveBetween((short)0, (short)4)
            .WithMessage(_ => Errors.CurrencyDecimalDigitsInvalid.GetMessage());

        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage(_ => Errors.CurrencySymbolIsRequired.GetMessage())
            .MaximumLength(10).WithMessage(_ => Errors.CurrencySymbolMaxLengthExceeded.GetMessage());
    }
}
