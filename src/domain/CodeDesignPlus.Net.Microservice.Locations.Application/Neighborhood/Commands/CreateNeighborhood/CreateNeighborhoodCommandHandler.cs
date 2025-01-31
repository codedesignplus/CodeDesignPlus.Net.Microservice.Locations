namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;

public class CreateNeighborhoodCommandHandler(INeighborhoodRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateNeighborhoodCommand>
{
    public Task Handle(CreateNeighborhoodCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}