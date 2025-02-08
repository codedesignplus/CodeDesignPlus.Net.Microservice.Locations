namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;

[DtoGenerator]
public record CreateTimezoneCommand(Guid Id, string Name) : IRequest;

public class Validator : AbstractValidator<CreateTimezoneCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
    }
}
