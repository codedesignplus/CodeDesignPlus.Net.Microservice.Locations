namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Commands.DeleteRegion;

[DtoGenerator]
public record DeleteRegionCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteRegionCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
