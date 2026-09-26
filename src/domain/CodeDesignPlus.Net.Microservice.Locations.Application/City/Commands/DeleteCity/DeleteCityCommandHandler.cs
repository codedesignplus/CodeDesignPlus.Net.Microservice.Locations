namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.DeleteCity;

public class DeleteCityCommandHandler(ICityRepository repository, IUserContext user, IPubSub pubsub, ILocalityRepository localities, ICacheManager cache) : IRequestHandler<DeleteCityCommand>
{
    public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<CityAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.CityNotFound);

        // Borrar un registro con hijos o en uso los dejaba huérfanos (plan 043 de pendings).
        ApplicationGuard.IsTrue(await localities.AnyByCityAsync(aggregate.Id, cancellationToken), Errors.CityHasLocalities);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<CityAggregate>(aggregate.Id,  cancellationToken);

        await cache.RemoveAsync(CacheKeys.ById(aggregate.Id));

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}