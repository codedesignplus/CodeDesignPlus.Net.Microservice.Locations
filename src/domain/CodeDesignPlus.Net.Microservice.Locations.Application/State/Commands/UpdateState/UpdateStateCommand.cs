namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;

[DtoGenerator]
public record UpdateStateCommand(Guid Id, Guid IdCountry, string Code, string Name, bool IsActive) : IRequest;

public class Validator : AbstractValidator<UpdateStateCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
