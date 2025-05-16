namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Commands.UpdateRegion;

[DtoGenerator]
public record UpdateRegionCommand(Guid Id, string Name, List<string> SubRegions, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateRegionCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull();
        RuleFor(x => x.SubRegions).NotEmpty().NotNull();
    }
}
