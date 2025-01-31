namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.UpdateLocality;

[DtoGenerator]
public record UpdateLocalityCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<UpdateLocalityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
