namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindNeighborhoodById;

public record FindNeighborhoodByIdQuery(Guid Id) : IRequest<NeighborhoodDto>;

public class Validator : AbstractValidator<FindNeighborhoodByIdQuery>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}