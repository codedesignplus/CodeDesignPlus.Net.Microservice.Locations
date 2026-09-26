namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;

[DtoGenerator]
public record CreateStateCommand(Guid Id, Guid IdCountry, string Code, string Name) : IRequest;

public class Validator : AbstractValidator<CreateStateCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IdCountry).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
    }
}
