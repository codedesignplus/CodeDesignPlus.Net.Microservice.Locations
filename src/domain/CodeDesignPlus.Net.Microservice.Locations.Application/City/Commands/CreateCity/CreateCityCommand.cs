namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;

[DtoGenerator]
public record CreateCityCommand(Guid Id, Guid IdState, string Name, string TimeZone) : IRequest;

public class Validator : AbstractValidator<CreateCityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.IdState).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.TimeZone).NotEmpty().NotNull().MaximumLength(100);
    }
}
