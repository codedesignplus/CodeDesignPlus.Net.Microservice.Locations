namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;

public class UpdateCityCommandHandler(ICityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateCityCommand>
{
    public Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}