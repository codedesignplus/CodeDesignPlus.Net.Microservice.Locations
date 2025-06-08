namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;

[DtoGenerator]
public record CreateCityCommand(Guid Id, Guid IdState, string Name, string Timezone) : IRequest;

public class Validator : AbstractValidator<CreateCityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.IdState).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.Timezone).MaximumLength(100);
    }
}
