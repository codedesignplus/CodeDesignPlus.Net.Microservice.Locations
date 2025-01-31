namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.UpdateLocality;

public class UpdateLocalityCommandHandler(ILocalityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateLocalityCommand>
{
    public Task Handle(UpdateLocalityCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}