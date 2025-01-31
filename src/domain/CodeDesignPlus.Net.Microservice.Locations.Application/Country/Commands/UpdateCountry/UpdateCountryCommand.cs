namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;

[DtoGenerator]
public record UpdateCountryCommand(Guid Id, string Name, string Code, Guid IdCurrency, string TimeZone, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateCountryCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(2);
        RuleFor(x => x.IdCurrency).NotEmpty().NotNull();
    }
}
