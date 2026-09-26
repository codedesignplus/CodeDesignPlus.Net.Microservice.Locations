namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.DeleteCurrency;

public class DeleteCurrencyCommandHandler(ICurrencyRepository repository, IUserContext user, IPubSub pubsub, ICountryRepository countries, ICacheManager cache) : IRequestHandler<DeleteCurrencyCommand>
{
    public async Task Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<CurrencyAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.CurrencyNotFound);

        // Borrar un registro con hijos o en uso los dejaba huérfanos (plan 043 de pendings).
        ApplicationGuard.IsTrue(await countries.AnyByCurrencyAsync(aggregate.Id, cancellationToken), Errors.CurrencyIsInUse);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<CurrencyAggregate>(aggregate.Id,  cancellationToken);

        await cache.RemoveAsync(CacheKeys.CurrencyById(aggregate.Id));


        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}