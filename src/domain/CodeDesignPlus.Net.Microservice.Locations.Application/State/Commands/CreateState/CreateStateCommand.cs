namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;

[DtoGenerator]
public record CreateStateCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<CreateStateCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
