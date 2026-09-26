namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.DeleteCountry;

public class DeleteCountryCommandHandler(ICountryRepository repository, IUserContext user, IPubSub pubsub, IStateRepository states, ICacheManager cache) : IRequestHandler<DeleteCountryCommand>
{
    public async Task Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<CountryAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.CountryNotFound);

        // Borrar un registro con hijos o en uso los dejaba huérfanos (plan 043 de pendings).
        ApplicationGuard.IsTrue(await states.AnyByCountryAsync(aggregate.Id, cancellationToken), Errors.CountryHasStates);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<CountryAggregate>(aggregate.Id,  cancellationToken);

        await cache.RemoveAsync(CacheKeys.CountryById(aggregate.Id));

        await cache.RemoveAsync(CacheKeys.AllCountries);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}