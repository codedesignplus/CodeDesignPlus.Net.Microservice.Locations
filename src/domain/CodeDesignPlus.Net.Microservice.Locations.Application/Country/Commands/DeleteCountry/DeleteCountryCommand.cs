namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.DeleteCountry;

[DtoGenerator]
public record DeleteCountryCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteCountryCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
