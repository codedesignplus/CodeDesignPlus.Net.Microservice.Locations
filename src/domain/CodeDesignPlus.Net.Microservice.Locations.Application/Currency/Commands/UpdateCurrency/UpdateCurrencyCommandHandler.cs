namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;

public class UpdateCurrencyCommandHandler(ICurrencyRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateCurrencyCommand>
{
    public async Task Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<CurrencyAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.CurrencyNotFound);

        aggregate.Update(request.Name, request.Code, request.Symbol, request.IsActive, user.IdUser);

        await repository.UpdateAsync<CurrencyAggregate>(aggregate, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}