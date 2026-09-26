namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.DeleteLocality;

public class DeleteLocalityCommandHandler(ILocalityRepository repository, IUserContext user, IPubSub pubsub, INeighborhoodRepository neighborhoods, ICacheManager cache) : IRequestHandler<DeleteLocalityCommand>
{
    public async Task Handle(DeleteLocalityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<LocalityAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.LocalityNotFound);

        // Borrar un registro con hijos o en uso los dejaba huérfanos (plan 043 de pendings).
        ApplicationGuard.IsTrue(await neighborhoods.AnyByLocalityAsync(aggregate.Id, cancellationToken), Errors.LocalityHasNeighborhoods);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<LocalityAggregate>(aggregate.Id,  cancellationToken);

        await cache.RemoveAsync(CacheKeys.ById(aggregate.Id));

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}