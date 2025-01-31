namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.UpdateNeighborhood;

[DtoGenerator]
public record UpdateNeighborhoodCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<UpdateNeighborhoodCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
