namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.UpdateNeighborhood;

public class UpdateNeighborhoodCommandHandler(INeighborhoodRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateNeighborhoodCommand>
{
    public async Task Handle(UpdateNeighborhoodCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<NeighborhoodAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.NeighborhoodNotFound);

        var existLocality = await repository.ExistsAsync<LocalityAggregate>(request.IdLocality,  cancellationToken);

        ApplicationGuard.IsFalse(existLocality, Errors.LocalityNotFound);

        aggregate.Update(request.IdLocality, request.Name, request.IsActive, user.IdUser);

        await repository.UpdateAsync(aggregate, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}