namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;

[DtoGenerator]
public record UpdateStateCommand(Guid Id, Guid IdCountry, string Code, string Name, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateStateCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IdCountry).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
    }
}
