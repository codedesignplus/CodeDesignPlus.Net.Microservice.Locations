namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;

[DtoGenerator]
public record UpdateCityCommand(Guid Id, Guid IdState, string Name, string TimeZone, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateCityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.IdState).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.TimeZone).NotEmpty().NotNull().MaximumLength(100);
    }
}
