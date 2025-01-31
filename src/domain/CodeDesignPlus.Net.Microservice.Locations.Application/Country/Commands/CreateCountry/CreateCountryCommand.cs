namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;

[DtoGenerator]
public record CreateCountryCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<CreateCountryCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
