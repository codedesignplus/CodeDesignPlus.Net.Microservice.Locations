namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.DeleteTimezone;

[DtoGenerator]
public record DeleteTimezoneCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteTimezoneCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
