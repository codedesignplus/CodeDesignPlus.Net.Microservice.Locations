namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.CreateLocality;

[DtoGenerator]
public record CreateLocalityCommand(Guid Id, string Name, Guid IdCity) : IRequest;

public class Validator : AbstractValidator<CreateLocalityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.IdCity).NotEmpty();
    }
}
