namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;

public class CreateCurrencyCommandHandler(ICurrencyRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateCurrencyCommand>
{
    public async Task Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exist = await repository.ExistsAsync<TimezoneAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.CurrencyAlreadyExists);

        var currency = CurrencyAggregate.Create(request.Id, request.Name, request.Code, request.Symbol, user.IdUser);

        await repository.CreateAsync(currency, cancellationToken);

        await pubsub.PublishAsync(currency.GetAndClearEvents(), cancellationToken);
    }
}