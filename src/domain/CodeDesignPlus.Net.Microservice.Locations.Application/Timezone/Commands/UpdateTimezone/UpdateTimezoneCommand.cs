namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.UpdateTimezone;

[DtoGenerator]
public record UpdateTimezoneCommand(Guid Id, string Name, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateTimezoneCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
    }
}
