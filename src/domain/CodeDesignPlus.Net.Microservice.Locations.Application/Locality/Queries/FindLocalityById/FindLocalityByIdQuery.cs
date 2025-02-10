namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindLocalityById;

public record FindLocalityByIdQuery(Guid Id) : IRequest<LocalityDto>;

public class Validator : AbstractValidator<FindLocalityByIdQuery>
{
    public Validator()
    {
        this.RuleFor(x => x.Id).NotEmpty();
    }
}