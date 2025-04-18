namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;

public class CreateNeighborhoodCommandHandler(INeighborhoodRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateNeighborhoodCommand>
{
    public async Task Handle(CreateNeighborhoodCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exist = await repository.ExistsAsync<NeighborhoodAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.NeighborhoodAlreadyExists);

        var existLocality = await repository.ExistsAsync<LocalityAggregate>(request.IdLocality,  cancellationToken);

        ApplicationGuard.IsFalse(existLocality, Errors.LocalityNotFound);

        var aggregate = NeighborhoodAggregate.Create(request.Id, request.IdLocality, request.Name, user.IdUser);

        await repository.CreateAsync(aggregate, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}