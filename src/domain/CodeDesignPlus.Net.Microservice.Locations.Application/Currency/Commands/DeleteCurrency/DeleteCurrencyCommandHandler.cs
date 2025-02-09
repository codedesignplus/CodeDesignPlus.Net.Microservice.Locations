namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.DeleteCurrency;

public class DeleteCurrencyCommandHandler(ICurrencyRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteCurrencyCommand>
{
    public async Task Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<CurrencyAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.CurrencyNotFound);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<CurrencyAggregate>(aggregate.Id,  cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}