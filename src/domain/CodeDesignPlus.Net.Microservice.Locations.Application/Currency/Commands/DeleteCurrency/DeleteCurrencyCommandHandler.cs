namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.DeleteCurrency;

public class DeleteCurrencyCommandHandler(ICurrencyRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteCurrencyCommand>
{
    public Task Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}