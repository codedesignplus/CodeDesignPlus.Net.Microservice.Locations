namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.DeleteNeighborhood;

public class DeleteNeighborhoodCommandHandler(INeighborhoodRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteNeighborhoodCommand>
{
    public Task Handle(DeleteNeighborhoodCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}