using CodeDesignPlus.Net.Microservice.Locations.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;

[DtoGenerator]
public record CreateTimezoneCommand(Guid Id, string Name, List<string> Aliases, Location Location, List<string> Offsets, string CurrentOffset, bool IsActive) : IRequest;

public class Validator : AbstractValidator<CreateTimezoneCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(128);
        RuleFor(x => x.Aliases).NotNull();
        RuleFor(x => x.Location).NotEmpty().NotNull();
        RuleFor(x => x.Offsets).NotEmpty().NotNull();
        RuleFor(x => x.CurrentOffset).NotEmpty().NotNull();
    }
}
