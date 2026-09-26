namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.DeleteTimezone;

public class DeleteTimezoneCommandHandler(ITimezoneRepository repository, IUserContext user, IPubSub pubsub, ICountryRepository countries, ICityRepository cities, ICacheManager cache) : IRequestHandler<DeleteTimezoneCommand>
{
    public async Task Handle(DeleteTimezoneCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<TimezoneAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.TimezoneNotFound);

        // Borrar un registro con hijos o en uso los dejaba huérfanos (plan 043 de pendings).
        var inUse = await countries.AnyByTimezoneAsync(aggregate.Name, cancellationToken) || await cities.AnyByTimezoneAsync(aggregate.Name, cancellationToken);

        ApplicationGuard.IsTrue(inUse, Errors.TimezoneIsInUse);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<TimezoneAggregate>(aggregate.Id,  cancellationToken);

        await cache.RemoveAsync(CacheKeys.ById(aggregate.Id));

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}