namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;

[DtoGenerator]
public record CreateCountryCommand(Guid Id, string Name, string Alpha2, string Alpha3, string Code, string? Capital, Guid IdCurrency, string Timezone, string NameNative, string Region, string SubRegion, double Latitude, double Longitude, string? Flag, bool IsActive) : IRequest;

public class Validator : AbstractValidator<CreateCountryCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Alpha2).NotEmpty().NotNull().MaximumLength(2);
        RuleFor(x => x.Alpha3).NotEmpty().NotNull().MaximumLength(3);
        RuleFor(x => x.Code).NotEmpty().NotNull();
        RuleFor(x => x.Capital).MaximumLength(100);
        RuleFor(x => x.IdCurrency).NotEmpty().NotNull();
        RuleFor(x => x.Timezone).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.NameNative).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Region).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.SubRegion).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Latitude).NotEmpty().NotNull();
        RuleFor(x => x.Longitude).NotEmpty().NotNull();
        RuleFor(x => x.Flag).MaximumLength(100);
    }
}
