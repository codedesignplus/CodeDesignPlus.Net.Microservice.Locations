namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.UpdateLocality;

[DtoGenerator]
public record UpdateLocalityCommand(Guid Id, string Name, Guid IdCity, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateLocalityCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
        RuleFor(x => x.IdCity).NotEmpty();
    }
}
