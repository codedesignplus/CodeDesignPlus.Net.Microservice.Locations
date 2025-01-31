namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;

[DtoGenerator]
public record CreateCityCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<CreateCityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
