namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindTimezoneById;

public record FindTimezoneByIdQuery(Guid Id) : IRequest<TimezoneDto>;

public class Validator : AbstractValidator<FindTimezoneByIdQuery>
{
    public Validator()
    {
        this.RuleFor(x => x.Id).NotEmpty();
    }
}