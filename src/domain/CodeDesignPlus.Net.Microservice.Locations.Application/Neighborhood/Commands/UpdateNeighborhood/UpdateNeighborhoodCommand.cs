namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.UpdateNeighborhood;

[DtoGenerator]
public record UpdateNeighborhoodCommand(Guid Id, string Name, Guid IdLocality, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateNeighborhoodCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        RuleFor(x => x.IdLocality).NotEmpty();
    }
}
