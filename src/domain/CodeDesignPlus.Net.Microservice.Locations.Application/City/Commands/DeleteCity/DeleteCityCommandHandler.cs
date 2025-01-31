namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.DeleteCity;

public class DeleteCityCommandHandler(ICityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteCityCommand>
{
    public Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}