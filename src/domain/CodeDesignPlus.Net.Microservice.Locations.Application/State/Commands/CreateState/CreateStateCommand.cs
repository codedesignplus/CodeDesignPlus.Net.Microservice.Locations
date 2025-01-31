namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;

[DtoGenerator]
public record CreateStateCommand(Guid Id, Guid IdCountry, string Code, string Name) : IRequest<Guid>;

public class Validator : AbstractValidator<CreateStateCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.IdCountry).NotEmpty().NotNull();
        RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(3);
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
    }
}
