namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.DeleteCity;

[DtoGenerator]
public record DeleteCityCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteCityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
