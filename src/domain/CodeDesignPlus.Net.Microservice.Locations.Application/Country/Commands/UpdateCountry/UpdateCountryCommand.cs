namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;

[DtoGenerator]
public record UpdateCountryCommand(Guid Id, string Name, string Alpha2, string Alpha3, string Code, string? Capital, Guid IdCurrency, string TimeZone, string NameNative, string Region, string SubRegion, double Latitude, double Longitude, string? Flag, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateCountryCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Alpha2).NotEmpty().NotNull().Length(2);
        RuleFor(x => x.Alpha3).NotEmpty().NotNull().Length(3);
        RuleFor(x => x.Capital).MaximumLength(100);
        RuleFor(x => x.IdCurrency).NotEmpty().NotNull();
        RuleFor(x => x.TimeZone).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.NameNative).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Region).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.SubRegion).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Latitude).NotEmpty().NotNull();
        RuleFor(x => x.Longitude).NotEmpty().NotNull();
        RuleFor(x => x.Flag).MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().NotNull();
    }
}
