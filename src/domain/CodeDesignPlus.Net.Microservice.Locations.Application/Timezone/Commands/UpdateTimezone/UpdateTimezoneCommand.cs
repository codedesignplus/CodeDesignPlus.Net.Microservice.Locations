namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.UpdateTimezone;

[DtoGenerator]
public record UpdateTimezoneCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<UpdateTimezoneCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
