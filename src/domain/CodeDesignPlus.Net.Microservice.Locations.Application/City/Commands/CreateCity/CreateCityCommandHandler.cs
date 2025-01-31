namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;

public class CreateCityCommandHandler(ICityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateCityCommand>
{
    public Task Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}