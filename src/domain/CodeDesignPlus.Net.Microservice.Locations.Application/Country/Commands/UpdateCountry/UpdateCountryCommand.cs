namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;

[DtoGenerator]
public record UpdateCountryCommand(Guid Id, string Name, string Alpha2, string Alpha3, string Code, string PhoneCode, string? Capital, Guid IdCurrency, string Timezone, string NameNative, string Region, string SubRegion, double Latitude, double Longitude, string? Flag, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateCountryCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Alpha2).NotEmpty().Length(2);
        RuleFor(x => x.Alpha3).NotEmpty().Length(3);
        RuleFor(x => x.PhoneCode).NotEmpty().Matches(@"^\+[0-9]{1,4}$").WithErrorCode(Errors.PhoneCodeFormatIsInvalid.Code);
        RuleFor(x => x.Capital).MaximumLength(100);
        RuleFor(x => x.IdCurrency).NotEmpty();
        RuleFor(x => x.Timezone).NotEmpty().MaximumLength(100);
        RuleFor(x => x.NameNative).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Region).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SubRegion).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        RuleFor(x => x.Flag).MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty();
    }
}
