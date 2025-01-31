namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.DeleteState;

public class DeleteStateCommandHandler(IStateRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteStateCommand>
{
    public Task Handle(DeleteStateCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}