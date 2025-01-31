namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.DeleteState;

[DtoGenerator]
public record DeleteStateCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<DeleteStateCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
