namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;

[DtoGenerator]
public record CreateCityCommand(Guid Id, Guid IdState, string Name, string Timezone) : IRequest;

public class Validator : AbstractValidator<CreateCityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IdState).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Timezone).NotEmpty().MaximumLength(100);
    }
}
