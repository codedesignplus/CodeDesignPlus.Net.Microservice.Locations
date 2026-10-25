namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;

public class CreateCurrencyCommandHandler(ICurrencyRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cache) : IRequestHandler<CreateCurrencyCommand>
{
    public async Task Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        // Ya guardado por un envío anterior del mismo formulario: el reintento termina bien sin repetir nada (pendings/589).
        if (await repository.ExistsAsync<CurrencyAggregate>(request.Id, cancellationToken))
            return;

        var currency = CurrencyAggregate.Create(request.Id, request.Code, request.NumericCode, request.DecimalDigits, request.Symbol, request.Name, user.IdUser);

        // Dos envíos que llegan a la vez pasan los dos la comprobación de arriba; el segundo choca contra el id y no publica.
        if (!await repository.CreateIfAbsentAsync(currency, cancellationToken))
            return;

        await cache.RemoveAsync(CacheKeys.CurrencyById(currency.Id));


        await pubsub.PublishAsync(currency.GetAndClearEvents(), cancellationToken);
    }
}