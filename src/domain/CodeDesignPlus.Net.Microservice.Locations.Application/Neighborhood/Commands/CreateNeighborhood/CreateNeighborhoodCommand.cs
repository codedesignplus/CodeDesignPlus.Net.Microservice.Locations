namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;

[DtoGenerator]
public record CreateNeighborhoodCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<CreateNeighborhoodCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
