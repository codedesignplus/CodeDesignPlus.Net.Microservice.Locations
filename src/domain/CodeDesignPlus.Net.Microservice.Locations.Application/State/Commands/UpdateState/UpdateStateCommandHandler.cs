namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;

public class UpdateStateCommandHandler(IStateRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateStateCommand>
{
    public Task Handle(UpdateStateCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}