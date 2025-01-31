namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.DeleteLocality;

[DtoGenerator]
public record DeleteLocalityCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteLocalityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
