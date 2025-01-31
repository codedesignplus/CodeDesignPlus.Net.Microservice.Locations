namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.UpdateNeighborhood;

public class UpdateNeighborhoodCommandHandler(INeighborhoodRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateNeighborhoodCommand>
{
    public Task Handle(UpdateNeighborhoodCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}