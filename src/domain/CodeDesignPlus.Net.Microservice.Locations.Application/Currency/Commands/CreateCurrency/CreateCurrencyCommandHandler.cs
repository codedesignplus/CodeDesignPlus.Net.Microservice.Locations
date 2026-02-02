namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;

public class CreateCurrencyCommandHandler(ICurrencyRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateCurrencyCommand>
{
    public async Task Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exist = await repository.ExistsAsync<CurrencyAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.CurrencyAlreadyExists);

        var currency = CurrencyAggregate.Create(request.Id, request.Code, request.NumericCode, request.DecimalDigits, request.Symbol, request.Name, user.IdUser);

        await repository.CreateAsync(currency, cancellationToken);

        await pubsub.PublishAsync(currency.GetAndClearEvents(), cancellationToken);
    }
}