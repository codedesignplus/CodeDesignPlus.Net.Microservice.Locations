namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.CreateLocality;

[DtoGenerator]
public record CreateLocalityCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<CreateLocalityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
