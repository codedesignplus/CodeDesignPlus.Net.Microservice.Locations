namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Commands.CreateRegion;

[DtoGenerator]
public record CreateRegionCommand(Guid Id, string Name, List<string> SubRegions, bool IsActive) : IRequest;

public class Validator : AbstractValidator<CreateRegionCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull();
        RuleFor(x => x.SubRegions).NotEmpty().NotNull();
    }
}
