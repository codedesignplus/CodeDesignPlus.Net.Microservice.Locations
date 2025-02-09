namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindStateById;

public record FindStateByIdQuery(Guid Id) : IRequest<StateDto>;

public class Validator : AbstractValidator<FindStateByIdQuery>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}