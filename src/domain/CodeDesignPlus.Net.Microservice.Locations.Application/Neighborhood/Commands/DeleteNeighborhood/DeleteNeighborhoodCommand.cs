namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.DeleteNeighborhood;

[DtoGenerator]
public record DeleteNeighborhoodCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteNeighborhoodCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
