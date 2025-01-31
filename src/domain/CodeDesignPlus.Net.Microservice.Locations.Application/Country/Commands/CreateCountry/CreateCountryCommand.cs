namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;

[DtoGenerator]
public record CreateCountryCommand(Guid Id, string Name, string Code, Guid IdCurrency, string TimeZone) : IRequest<Guid>;

public class Validator : AbstractValidator<CreateCountryCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(2);
        RuleFor(x => x.IdCurrency).NotEmpty().NotNull();
    }
}
