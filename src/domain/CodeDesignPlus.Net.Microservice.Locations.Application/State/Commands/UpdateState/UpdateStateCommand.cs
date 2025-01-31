namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;

[DtoGenerator]
public record UpdateStateCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<UpdateStateCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
