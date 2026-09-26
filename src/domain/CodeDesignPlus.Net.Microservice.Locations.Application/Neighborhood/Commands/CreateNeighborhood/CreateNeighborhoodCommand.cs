namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;

[DtoGenerator]
public record CreateNeighborhoodCommand(Guid Id, string Name, Guid IdLocality) : IRequest;

public class Validator : AbstractValidator<CreateNeighborhoodCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        RuleFor(x => x.IdLocality).NotEmpty();
    }
}
