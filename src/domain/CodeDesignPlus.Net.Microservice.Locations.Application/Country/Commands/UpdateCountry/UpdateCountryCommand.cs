namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;

[DtoGenerator]
public record UpdateCountryCommand(Guid Id, string Name, string Alpha2, string Alpha3, ushort Code, string? Capital, Guid IdCurrency, string TimeZone, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateCountryCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Alpha2).NotEmpty().NotNull().Length(2);
        RuleFor(x => x.Alpha3).NotEmpty().NotNull().Length(3);
        RuleFor(x => x.Code).NotEmpty().NotNull().GreaterThan((ushort)0);
        RuleFor(x => x.Capital).MaximumLength(100);
        RuleFor(x => x.IdCurrency).NotEmpty().NotNull();
    }
}
