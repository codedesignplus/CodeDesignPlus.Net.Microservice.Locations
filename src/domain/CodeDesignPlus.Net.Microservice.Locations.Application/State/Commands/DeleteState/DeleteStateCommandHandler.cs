namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.DeleteState;

public class DeleteStateCommandHandler(IStateRepository repository, IUserContext user, IPubSub pubsub, ICityRepository cities, ICacheManager cache) : IRequestHandler<DeleteStateCommand>
{
    public async Task Handle(DeleteStateCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<StateAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.StateNotFound);

        // Borrar un registro con hijos o en uso los dejaba huérfanos (plan 043 de pendings).
        ApplicationGuard.IsTrue(await cities.AnyByStateAsync(aggregate.Id, cancellationToken), Errors.StateHasCities);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<StateAggregate>(aggregate.Id,  cancellationToken);

        await cache.RemoveAsync(CacheKeys.ById(aggregate.Id));

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}