namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;

[DtoGenerator]
public record CreateNeighborhoodCommand(Guid Id, string Name, Guid IdLocality) : IRequest;

public class Validator : AbstractValidator<CreateNeighborhoodCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(x => x.IdLocality).NotEmpty().NotNull();
    }
}
