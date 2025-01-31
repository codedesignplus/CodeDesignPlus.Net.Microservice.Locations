namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;

[DtoGenerator]
public record UpdateCityCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<UpdateCityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
