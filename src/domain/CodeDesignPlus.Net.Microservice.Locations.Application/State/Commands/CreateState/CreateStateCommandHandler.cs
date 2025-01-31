namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;

public class CreateStateCommandHandler(IStateRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateStateCommand>
{
    public Task Handle(CreateStateCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}