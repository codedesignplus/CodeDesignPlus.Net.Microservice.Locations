namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.DeleteNeighborhood;

public class DeleteNeighborhoodCommandHandler(INeighborhoodRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cache) : IRequestHandler<DeleteNeighborhoodCommand>
{
    public async Task Handle(DeleteNeighborhoodCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<NeighborhoodAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.NeighborhoodNotFound);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<NeighborhoodAggregate>(aggregate.Id,  cancellationToken);

        await cache.RemoveAsync(CacheKeys.ById(aggregate.Id));

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}