namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;

[DtoGenerator]
public record UpdateCityCommand(Guid Id, Guid IdState, string Name, string Timezone, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateCityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IdState).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Timezone).NotEmpty().MaximumLength(100);
    }
}
