namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.DeleteLocality;

public class DeleteLocalityCommandHandler(ILocalityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteLocalityCommand>
{
    public Task Handle(DeleteLocalityCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}