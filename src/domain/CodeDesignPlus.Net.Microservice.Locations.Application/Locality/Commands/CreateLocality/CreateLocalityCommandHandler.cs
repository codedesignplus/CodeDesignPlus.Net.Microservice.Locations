namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.CreateLocality;

public class CreateLocalityCommandHandler(ILocalityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateLocalityCommand>
{
    public Task Handle(CreateLocalityCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}